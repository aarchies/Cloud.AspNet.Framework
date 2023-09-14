# Protobuf3 语言指南

## 出处

* 英文原文：[Language Guide\(proto3\)](https://developers.google.com/protocol-buffers/docs/proto3?hl=zh-cn#generating)
* 中文出处：[Protobuf 语言指南](https://github.com/lixiangyun/protobuf_doc_ZH_CN)

## 目录

* [定义一个消息类型](#定义一个消息类型)
* [标量数值类型](#标量数值类型)
* [默认值](#默认值)
* [枚举](#枚举)
* [使用其他消息类型](#使用其他消息类型)
* [嵌套类型](#嵌套类型)
* [更新一个消息类型](#更新一个消息类型)
* [Any](#any)
* [Oneof](#oneof)
* [映射（Maps）](#映射maps)
* [包（Packages）](#包packages)
* [定义服务](#定义服务)
* [JSON 映射](#json-映射)
* [选项](#选项)
* [生成你的类](#生成你的类)

## 定义一个消息类型

先来看一个非常简单的例子。假设你想定义一个“搜索请求”的消息格式，每一个请求含有一个查询字符串、你感兴趣的查询结果所在的页数，以及每一页多少条查询结果。可以采用如下的方式来定义消息类型的.proto文件了：

```
syntax = "proto3";

message SearchRequest {
  string query = 1;
  int32 page_number = 2;
  int32 result_per_page = 3;
}
```

* 文件的第一行指定了你正在使用proto3语法：如果你没有指定这个，编译器会使用proto2。这个指定语法行必须是文件的非空非注释的第一个行。
* SearchRequest消息格式有3个字段，在消息中承载的数据分别对应于每一个字段。其中每个字段都有一个名字和一种类型。

### 指定字段类型

在上面的例子中，所有字段都是标量类型：两个整型（page\_number和result\_per\_page），一个string类型（query）。当然，你也可以为字段指定其他的合成类型，包括枚举（enumerations）或其他消息类型。

### 分配标识号

正如你所见，在消息定义中，每个字段都有唯一的一个数字标识符。这些标识符是用来在消息的二进制格式中识别各个字段的，一旦开始使用就不能够再改变。注：\[1,15\]之内的标识号在编码的时候会占用一个字节。\[16,2047\]之内的标识号则占用2个字节。所以应该为那些频繁出现的消息元素保留 \[1,15\]之内的标识号。切记：要为将来有可能添加的、频繁出现的标识号预留一些标识号。

最小的标识号可以从1开始，最大到2^29 - 1, or 536,870,911。不可以使用其中的\[19000－19999\]（ \(从FieldDescriptor::kFirstReservedNumber 到 FieldDescriptor::kLastReservedNumber\)）的标识号， Protobuf协议实现中对这些进行了预留。如果非要在.proto文件中使用这些预留标识号，编译时就会报警。同样你也不能使用早期保留的标识号。

### 指定字段规则

所指定的消息字段修饰符必须是如下之一：

* singular：一个格式良好的消息应该有0个或者1个这种字段（但是不能超过1个）。
* repeated：在一个格式良好的消息中，这种字段可以重复任意多次（包括0次）。重复的值的顺序会被保留。

在proto3中，repeated的标量域默认情况虾使用packed。

你可以了解更多的pakced属性在Protocol Buffer 编码。

### 添加更多消息类型

在一个.proto文件中可以定义多个消息类型。在定义多个相关的消息的时候，这一点特别有用——例如，如果想定义与SearchResponse消息类型对应的回复消息格式的话，你可以将它添加到相同的.proto文件中，如：

```
message SearchRequest {
  string query = 1;
  int32 page_number = 2;
  int32 result_per_page = 3;
}

message SearchResponse {
 ...
}
```

### 添加注释

向.proto文件添加注释，可以使用C/C++/java风格的双斜杠（//） 语法格式，如：

```
message SearchRequest {
  string query = 1;
  int32 page_number = 2;  // Which page number do we want?
  int32 result_per_page = 3;  // Number of results to return per page.
}
```

### 保留标识符（Reserved）

如果你通过删除或者注释所有域，以后的用户可以重用标识号当你重新更新类型的时候。如果你使用旧版本加载相同的.proto文件这会导致严重的问题，包括数据损坏、隐私错误等等。现在有一种确保不会发生这种情况的方法就是指定保留标识符（and/or names, which can also cause issues for JSON serialization不明白什么意思），protocol buffer的编译器会警告未来尝试使用这些域标识符的用户。

```
message Foo {
  reserved 2, 15, 9 to 11;
  reserved "foo", "bar";
}
```

注：不要在同一行reserved声明中同时声明域名字和标识号

### 从.proto文件生成了什么？

当用protocol buffer编译器来运行.proto文件时，编译器将生成所选择语言的代码，这些代码可以操作在.proto文件中定义的消息类型，包括获取、设置字段值，将消息序列化到一个输出流中，以及从一个输入流中解析消息。

* 对C++来说，编译器会为每个.proto文件生成一个.h文件和一个.cc文件，.proto文件中的每一个消息有一个对应的类。
* 对Java来说，编译器为每一个消息类型生成了一个.java文件，以及一个特殊的Builder类（该类是用来创建消息类接口的）。
* 对Python来说，有点不太一样——Python编译器为.proto文件中的每个消息类型生成一个含有静态描述符的模块，，该模块与一个元类（metaclass）在运行时（runtime）被用来创建所需的Python数据访问类。
* 对go来说，编译器会位每个消息类型生成了一个.pd.go文件。
* 对于Ruby来说，编译器会为每个消息类型生成了一个.rb文件。
* javaNano来说，编译器输出类似域java但是没有Builder类
* 对于Objective-C来说，编译器会为每个消息类型生成了一个pbobjc.h文件和pbobjcm文件，.proto文件中的每一个消息有一个对应的类。
* 对于C\#来说，编译器会为每个消息类型生成了一个.cs文件，.proto文件中的每一个消息有一个对应的类。

你可以从如下的文档链接中获取每种语言更多API\(proto3版本的内容很快就公布\)。[API Reference](https://developers.google.com/protocol-buffers/docs/reference/overview?hl=zh-cn)

## 标量数值类型

一个标量消息字段可以含有一个如下的类型——该表格展示了定义于.proto文件中的类型，以及与之对应的、在自动生成的访问类中定义的类型：

| .proto Type | Notes | C++ Type | Java Type | Python Type\[2\] | Go Type | Ruby Type | C\# Type | PHP Type |
| --- | ------------ | --- | --- | --- | --- | --- | --- | --- |
| double |  | double | double | float | float64 | Float | double | float |
| float |  | float | float | float | float32 | Float | float | float |
| int32 | 使用变长编码，对于负值的效率很低，如果你的域有可能有负值，请使用sint64替代 | int32 | int | int | int32 | Fixnum 或者 Bignum（根据需要） | int | integer |
| uint32 | 使用变长编码 | uint32 | int | int/long | uint32 | Fixnum 或者 Bignum（根据需要） | uint | integer |
| uint64 | 使用变长编码 | uint64 | long | int/long | uint64 | Bignum | ulong | integer/string |
| sint32 | 使用变长编码，这些编码在负值时比int32高效的多 | int32 | int | int | int32 | Fixnum 或者 Bignum（根据需要） | int | integer |
| sint64 | 使用变长编码，有符号的整型值。编码时比通常的int64高效。 | int64 | long | int/long | int64 | Bignum | long | integer/string |
| fixed32 | 总是4个字节，如果数值总是比总是比228大的话，这个类型会比uint32高效。 | uint32 | int | int | uint32 | Fixnum 或者 Bignum（根据需要） | uint | integer |
| fixed64 | 总是8个字节，如果数值总是比总是比256大的话，这个类型会比uint64高效。 | uint64 | long | int/long | uint64 | Bignum | ulong | integer/string |
| sfixed32 | 总是4个字节 | int32 | int | int | int32 | Fixnum 或者 Bignum（根据需要） | int | integer |
| sfixed64 | 总是8个字节 | int64 | long | int/long | int64 | Bignum | long | integer/string |
| bool |  | bool | boolean | bool | bool | TrueClass/FalseClass | bool | boolean |
| string | 一个字符串必须是UTF-8编码或者7-bit ASCII编码的文本。 | string | String | str/unicode | string | String \(UTF-8\) | string | string |
| bytes | 可能包含任意顺序的字节数据。 | string | ByteString | str | \[\]byte | String \(ASCII-8BIT\) | ByteString | string |

## 默认值

当一个消息被解析的时候，如果被编码的信息不包含一个特定的singular元素，被解析的对象锁对应的域被设置位一个默认值，对于不同类型指定如下：

* 对于strings，默认是一个空string
* 对于bytes，默认是一个空的bytes
* 对于bools，默认是false
* 对于数值类型，默认是0
* 对于枚举，默认是第一个定义的枚举值，必须为0;
* 对于消息类型（message），域没有被设置，确切的消息是根据语言确定的，详见[generated code guide](https://developers.google.com/protocol-buffers/docs/reference/overview?hl=zh-cn)。

对于可重复域的默认值是空（通常情况下是对应语言中空列表）。

注：对于标量消息域，一旦消息被解析，就无法判断域释放被设置为默认值（例如，例如boolean值是否被设置为false）还是根本没有被设置。你应该在定义你的消息类型时非常注意。例如，比如你不应该定义boolean的默认值false作为任何行为的触发方式。也应该注意如果一个标量消息域被设置为标志位，这个值不应该被序列化传输。

查看[generated code guide](https://developers.google.com/protocol-buffers/docs/reference/overview?hl=zh-cn)选择你的语言的默认值的工作细节。

## 枚举

当需要定义一个消息类型的时候，可能想为一个字段指定某“预定义值序列”中的一个值。例如，假设要为每一个SearchRequest消息添加一个 corpus字段，而corpus的值可能是UNIVERSAL，WEB，IMAGES，LOCAL，NEWS，PRODUCTS或VIDEO中的一个。 其实可以很容易地实现这一点：通过向消息定义中添加一个枚举（enum）并且为每个可能的值定义一个常量就可以了。

在下面的例子中，在消息格式中添加了一个叫做Corpus的枚举类型——它含有所有可能的值 ——以及一个类型为Corpus的字段：

```
message SearchRequest {
  string query = 1;
  int32 page_number = 2;
  int32 result_per_page = 3;
  enum Corpus {
    UNIVERSAL = 0;
    WEB = 1;
    IMAGES = 2;
    LOCAL = 3;
    NEWS = 4;
    PRODUCTS = 5;
    VIDEO = 6;
  }
  Corpus corpus = 4;
}
```

如你所见，Corpus枚举的第一个常量映射为0：每个枚举类型必须将其第一个类型映射为0，这是因为：

* 必须有有一个0值，我们可以用这个0值作为默认值。
* 这个零值必须为第一个元素，为了兼容proto2语义，枚举类的第一个值总是默认值。

你可以通过将不同的枚举常量指定位相同的值。如果这样做你需要将allow\_alias设定位true，否则编译器会在别名的地方产生一个错误信息。

```
enum EnumAllowingAlias {
  option allow_alias = true;
  UNKNOWN = 0;
  STARTED = 1;
  RUNNING = 1;
}
enum EnumNotAllowingAlias {
  UNKNOWN = 0;
  STARTED = 1;
  // RUNNING = 1;  // Uncommenting this line will cause a compile error inside Google and a warning message outside.
}
```

枚举常量必须在32位整型值的范围内。因为enum值是使用可变编码方式的，对负数不够高效，因此不推荐在enum中使用负数。如上例所示，可以在 一个消息定义的内部或外部定义枚举——这些枚举可以在.proto文件中的任何消息定义里重用。当然也可以在一个消息中声明一个枚举类型，而在另一个不同 的消息中使用它——采用MessageType.EnumType的语法格式。

当对一个使用了枚举的.proto文件运行protocol buffer编译器的时候，生成的代码中将有一个对应的enum（对Java或C++来说），或者一个特殊的EnumDescriptor类（对 Python来说），它被用来在运行时生成的类中创建一系列的整型值符号常量（symbolic constants）。

在反序列化的过程中，无法识别的枚举值会被保存在消息中，虽然这种表示方式需要依据所使用语言而定。在那些支持开放枚举类型超出指定范围之外的语言中（例如C++和Go），为识别的值会被表示成所支持的整型。在使用封闭枚举类型的语言中（Java），使用枚举中的一个类型来表示未识别的值，并且可以使用所支持整型来访问。在其他情况下，如果解析的消息被序列号，未识别的值将保持原样。

关于如何在你的应用程序的消息中使用枚举的更多信息，请查看所选择的语言[generated code guide](http://code.google.com/intl/zh-CN/apis/protocolbuffers/docs/reference/overview.html。)

## 使用其他消息类型

你可以将其他消息类型用作字段类型。例如，假设在每一个SearchResponse消息中包含Result消息，此时可以在相同的.proto文件中定义一个Result消息类型，然后在SearchResponse消息中指定一个Result类型的字段，如：

```
message SearchResponse {
  repeated Result results = 1;
}

message Result {
  string url = 1;
  string title = 2;
  repeated string snippets = 3;
}
```

### 导入定义

在上面的例子中，Result消息类型与SearchResponse是定义在同一文件中的。如果想要使用的消息类型已经在其他.proto文件中已经定义过了呢？   
你可以通过导入（importing）其他.proto文件中的定义来使用它们。要导入其他.proto文件的定义，你需要在你的文件中添加一个导入声明，如：

```
import "myproject/other_protos.proto";
```

默认情况下你只能使用直接导入的.proto文件中的定义. 然而， 有时候你需要移动一个.proto文件到一个新的位置， 可以不直接移动.proto文件， 只需放入一个伪 .proto 文件在老的位置， 然后使用import public转向新的位置。import public 依赖性会通过任意导入包含import public声明的proto文件传递。例如：

```
// 这是新的proto
// All definitions are moved here
```

```
// 这是久的proto
// 这是所有客户端正在导入的包
import public "new.proto";
import "other.proto";
```

```
// 客户端proto
import "old.proto";
// 现在你可以使用新久两种包的proto定义了。
```

通过在编译器命令行参数中使用-I/--proto\_pathprotocal 编译器会在指定目录搜索要导入的文件。如果没有给出标志，编译器会搜索编译命令被调用的目录。通常你只要指定proto\_path标志为你的工程根目录就好。并且指定好导入的正确名称就好。

### 使用proto2消息类型

在你的proto3消息中导入proto2的消息类型也是可以的，反之亦然，然后proto2枚举不可以直接在proto3的标识符中使用（如果仅仅在proto2消息中使用是可以的）。

## 嵌套类型

你可以在其他消息类型中定义、使用消息类型，在下面的例子中，Result消息就定义在SearchResponse消息内，如：

```
message SearchResponse {
  message Result {
    string url = 1;
    string title = 2;
    repeated string snippets = 3;
  }
  repeated Result results = 1;
}
```

如果你想在它的父消息类型的外部重用这个消息类型，你需要以Parent.Type的形式使用它，如：

```
message SomeOtherMessage {
  SearchResponse.Result result = 1;
}
```

当然，你也可以将消息嵌套任意多层，如：

```
message Outer {                  // Level 0
  message MiddleAA {  // Level 1
    message Inner {   // Level 2
      int64 ival = 1;
      bool  booly = 2;
    }
  }
  message MiddleBB {  // Level 1
    message Inner {   // Level 2
      int32 ival = 1;
      bool  booly = 2;
    }
  }
}
```

## 更新一个消息类型

如果一个已有的消息格式已无法满足新的需求——如，要在消息中添加一个额外的字段——但是同时旧版本写的代码仍然可用。不用担心！更新消息而不破坏已有代码是非常简单的。在更新时只要记住以下的规则即可。

* 不要更改任何已有的字段的数值标识。
* 如果你增加新的字段，使用旧格式的字段仍然可以被你新产生的代码所解析。你应该记住这些元素的默认值这样你的新代码就可以以适当的方式和旧代码产生的数据交互。相似的，通过新代码产生的消息也可以被旧代码解析：只不过新的字段会被忽视掉。注意，未被识别的字段会在反序列化的过程中丢弃掉，所以如果消息再被传递给新的代码，新的字段依然是不可用的（这和proto2中的行为是不同的，在proto2中未定义的域依然会随着消息被序列化）
* 非required的字段可以移除——只要它们的标识号在新的消息类型中不再使用（更好的做法可能是重命名那个字段，例如在字段前添加“OBSOLETE\_”前缀，那样的话，使用的.proto文件的用户将来就不会无意中重新使用了那些不该使用的标识号）。
* int32, uint32, int64, uint64,和bool是全部兼容的，这意味着可以将这些类型中的一个转换为另外一个，而不会破坏向前、 向后的兼容性。如果解析出来的数字与对应的类型不相符，那么结果就像在C++中对它进行了强制类型转换一样（例如，如果把一个64位数字当作int32来 读取，那么它就会被截断为32位的数字）。
* sint32和sint64是互相兼容的，但是它们与其他整数类型不兼容。
* string和bytes是兼容的——只要bytes是有效的UTF-8编码。
* 嵌套消息与bytes是兼容的——只要bytes包含该消息的一个编码过的版本。
* fixed32与sfixed32是兼容的，fixed64与sfixed64是兼容的。
* 枚举类型与int32，uint32，int64和uint64相兼容（注意如果值不相兼容则会被截断），然而在客户端反序列化之后他们可能会有不同的处理方式，例如，未识别的proto3枚举类型会被保留在消息中，但是他的表示方式会依照语言而定。int类型的字段总会保留他们的。

## Any

Any类型消息允许你在没有指定他们的.proto定义的情况下使用消息作为一个嵌套类型。一个Any类型包括一个可以被序列化bytes类型的任意消息，以及一个URL作为一个全局标识符和解析消息类型。为了使用Any类型，你需要导入import google/protobuf/any.proto

```
import "google/protobuf/any.proto";

message ErrorStatus {
  string message = 1;
  repeated google.protobuf.Any details = 2;
}
```

对于给定的消息类型的默认类型URL是type.googleapis.com/packagename.messagename。

不同语言的实现会支持动态库以线程安全的方式去帮助封装或者解封装Any值。例如在java中，Any类型会有特殊的pack\(\)和unpack\(\)访问器，在C++中会有PackFrom\(\)和UnpackTo\(\)方法。

```
// Storing an arbitrary message type in Any.
NetworkErrorDetails details = ...;
ErrorStatus status;
status.add_details()->PackFrom(details);

// Reading an arbitrary message from Any.
ErrorStatus status = ...;
for (const Any& detail : status.details()) {
  if (detail.Is<NetworkErrorDetails>()) {
    NetworkErrorDetails network_error;
    detail.UnpackTo(&network_error);
    ... processing network_error ...
  }
}
```

目前，用于Any类型的动态库仍在开发之中   
如果你已经很熟悉proto2语法，使用Any替换拓展

## Oneof

如果你的消息中有很多可选字段， 并且同时至多一个字段会被设置， 你可以加强这个行为，使用 oneof 特性节省内存.

Oneof 字段就像可选字段， 除了它们会共享内存， 至多一个字段会被设置。 设置其中一个字段会清除其它字段。 你可以使用 `case()` 或者 `WhichOneof()` 方法检查哪个 oneof 字段被设置， 看你使用什么语言了.

### 使用Oneof

为了在 `.proto` 定义 Oneof 字段， 你需要在名字前面加上 `oneof` 关键字, 比如下面例子的 `test_oneof`:

```c++
message SampleMessage {
  oneof test_oneof {
    string name = 4;
    SubMessage sub_message = 9;
  }
}
```

然后你可以增加 oneof 字段到 oneof 定义中. 你可以增加任意类型的字段, `map` 字段和 `repeated` 字段除外。

在产生的代码中, oneof字段拥有同样的 getters 和 setters， 就像正常的可选字段一样. 也有一个特殊的方法来检查到底那个字段被设置. 你可以在相应的语言[API指南](https://developers.google.com/protocol-buffers/docs/reference/overview)中找到oneof API介绍.

### Oneof 特性

* 设置 oneof 会自动清除其它 oneof 字段的值. 所以设置多次后，只有最后一次设置的字段有值.

```c++
SampleMessage message;
message.set_name("name");
CHECK(message.has_name());
message.mutable_sub_message();   // Will clear name field.
CHECK(!message.has_name());
```

* 如果解析器遇到同一个 oneof 中有多个成员，只有最会一个会被解析成消息。
* oneof 不支持 `repeated`.
* 反射 API 对 oneof 字段有效.
* 如果使用 C++,需确保代码不会导致内存泄漏. 下面的代码会崩溃， 因为 `sub_message` 已经通过 `set_name()` 删除了

```c++
SampleMessage message;
SubMessage* sub_message = message.mutable_sub_message();
message.set_name("name");      // Will delete sub_message
sub_message->set_...            // Crashes here
```

* 在C++中，如果你使用 `Swap()` 两个 oneof 消息，每个消息，两个消息将拥有对方的值，例如在下面的例子中，`msg1` 会拥有 `sub_message` 并且 `msg2` 会有 `name`。

```c++
SampleMessage msg1;
msg1.set_name("name");
SampleMessage msg2;
msg2.mutable_sub_message();
msg1.swap(&msg2);
CHECK(msg1.has_sub_message());
CHECK(msg2.has_name());
```

### 向后兼容性问题

当增加或者删除 oneof 字段时一定要小心. 如果检查 oneof 的值返回 `None`/`NOT_SET` , 它意味着 `oneof` 字段没有被赋值或者在一个不同的版本中赋值了。 你不会知道是哪种情况，因为没有办法判断如果未识别的字段是一个 `oneof` 字段。

Tage 重用问题：

* __将字段移入或移除 oneof__：在消息被序列号或者解析后，你也许会失去一些信息（有些字段也许会被清除）
* __删除一个字段或者加入一个字段__：在消息被序列号或者解析后，这也许会清除你现在设置的 oneof 字段
* __拆分或合并 oneof__：行为与移动常规字段相似。

## 映射（Maps）

如果你希望创建一个关联映射，protocol buffer提供了一种快捷的语法：

```
map<key_type, value_type> map_field = N;
```

其中key\_type可以是任意Integer或者string类型（所以，除了floating和bytes的任意标量类型都是可以的）value\_type可以是任意类型。

例如，如果你希望创建一个project的映射，每个Projecct使用一个string作为key，你可以像下面这样定义：

```
map<string, Project> projects = 3;
```

* Map的字段可以是repeated。
* 序列化后的顺序和map迭代器的顺序是不确定的，所以你不要期望以固定顺序处理Map
* 当为.proto文件产生生成文本格式的时候，map会按照key 的顺序排序，数值化的key会按照数值排序。
* 从序列化中解析或者融合时，如果有重复的key则后一个key不会被使用，当从文本格式中解析map时，如果存在重复的key。

生成map的API现在对于所有proto3支持的语言都可用了，你可以从API指南找到更多信息。

### 向后兼容性问题

map语法序列化后等同于如下内容，因此即使是不支持map语法的protocol buffer实现也是可以处理你的数据的：

```
message MapFieldEntry {
  key_type key = 1;
  value_type value = 2;
}

repeated MapFieldEntry map_field = N;
```

## 包（Packages）

当然可以为.proto文件新增一个可选的package声明符，用来防止不同的消息类型有命名冲突。如：

package foo.bar;  
message Open { ... }

在其他的消息格式定义中可以使用包名+消息名的方式来定义域的类型，如：

```
message Foo {
  ...
  required foo.bar.Open open = 1;
  ...
}
```

包的声明符会根据使用语言的不同影响生成的代码。

* 对于C++，产生的类会被包装在C++的命名空间中，如上例中的Open会被封装在 foo::bar空间中； - 对于Java，包声明符会变为java的一个包，除非在.proto文件中提供了一个明确有java\_package；
* 对于 Python，这个包声明符是被忽略的，因为Python模块是按照其在文件系统中的位置进行组织的。
* 对于Go，包可以被用做Go包名称，除非你显式的提供一个option go\_package在你的.proto文件中。
* 对于Ruby，生成的类可以被包装在内置的Ruby名称空间中，转换成Ruby所需的大小写样式 （首字母大写；如果第一个符号不是一个字母，则使用PB\_前缀），例如Open会在Foo::Bar名称空间中。
* 对于javaNano包会使用Java包，除非你在你的文件中显式的提供一个option java\_package。
* 对于C\#包可以转换为PascalCase后作为名称空间，除非你在你的文件中显式的提供一个option csharp\_namespace，例如，Open会在Foo.Bar名称空间中

### 包及名称的解析

Protocol buffer语言中类型名称的解析与C++是一致的：首先从最内部开始查找，依次向外进行，每个包会被看作是其父类包的内部类。当然对于 （foo.bar.Baz）这样以“.”分隔的意味着是从最外围开始的。

ProtocolBuffer编译器会解析.proto文件中定义的所有类型名。 对于不同语言的代码生成器会知道如何来指向每个具体的类型，即使它们使用了不同的规则。

## 定义服务

如果想要将消息类型用在RPC\(远程方法调用\)系统中，可以在.proto文件中定义一个RPC服务接口，protocol buffer编译器将会根据所选择的不同语言生成服务接口代码及存根。如，想要定义一个RPC服务并具有一个方法，该方法能够接收 SearchRequest并返回一个SearchResponse，此时可以在.proto文件中进行如下定义：

```
service SearchService {
  rpc Search (SearchRequest) returns (SearchResponse);
}
```

最直观的使用protocol buffer的RPC系统是gRPC一个由谷歌开发的语言和平台中的开源的PRC系统，gRPC在使用protocl buffer时非常有效，如果使用特殊的protocol buffer插件可以直接为您从.proto文件中产生相关的RPC代码。

如果你不想使用gRPC，也可以使用protocol buffer用于自己的RPC实现，你可以从proto2语言指南中找到更多信息

还有一些第三方开发的PRC实现使用Protocol Buffer。参考第三方插件wiki查看这些实现的列表。

## JSON 映射

Proto3 支持JSON的编码规范，使他更容易在不同系统之间共享数据，在下表中逐个描述类型。

如果JSON编码的数据丢失或者其本身就是null，这个数据会在解析成protocol buffer的时候被表示成默认值。如果一个字段在protocol buffer中表示为默认值，体会在转化成JSON的时候编码的时候忽略掉以节省空间。具体实现可以提供在JSON编码中可选的默认值。

| proto3 | JSON | JSON示例 | 注意 |
| --- | --- | --- | --- |
| message | object | {“fBar”: v, “g”: null, …} | 产生JSON对象，消息字段名可以被映射成lowerCamelCase形式，并且成为JSON对象键，null被接受并成为对应字段的默认值 |
| enum | string | “FOO\_BAR” | 枚举值的名字在proto文件中被指定 |
| map | object | {“k”: v, …} | 所有的键都被转换成string |
| repeated V | array | \[v, …\] | null被视为空列表 |
| bool | true, false | true, false |  |
| string | string | “Hello World!” |  |
| bytes | base64 string | “YWJjMTIzIT8kKiYoKSctPUB+” |  |
| int32, fixed32, uint32 | number | 1, -10, 0 | JSON值会是一个十进制数，数值型或者string类型都会接受 |
| int64, fixed64, uint64 | string | “1”, “-10” | JSON值会是一个十进制数，数值型或者string类型都会接受 |
| float, double | number | 1.1, -10.0, 0, “NaN”, “Infinity” | JSON值会是一个数字或者一个指定的字符串如”NaN”,”infinity”或者”-Infinity”，数值型或者字符串都是可接受的，指数符号也可以接受 |
| Any | object | {“@type”: “url”, “f”: v, … } | 如果一个Any保留一个特上述的JSON映射，则它会转换成一个如下形式：{"@type": xxx, "value": yyy}否则，该值会被转换成一个JSON对象，@type字段会被插入所指定的确定的值 |
| Timestamp | string | “1972-01-01T10:00:20.021Z” | 使用RFC 339，其中生成的输出将始终是Z-归一化啊的，并且使用0，3，6或者9位小数 |
| Duration | string | “1.000340012s”, “1s” | 生成的输出总是0，3，6或者9位小数，具体依赖于所需要的精度，接受所有可以转换为纳秒级的精度 |
| Struct | object | { … } | 任意的JSON对象，见struct.proto |
| Wrapper types | various types | 2, “2”, “foo”, true, “true”, null, 0, … | 包装器在JSON中的表示方式类似于基本类型，但是允许nulll，并且在转换的过程中保留null |
| FieldMask | string | “f.fooBar,h” | 见fieldmask.proto |
| ListValue | array | \[foo, bar, …\] |  |
| Value | value |  | 任意JSON值 |
| NullValue | null |  | JSON null |

## 选项

在定义.proto文件时能够标注一系列的options。Options并不改变整个文件声明的含义，但却能够影响特定环境下处理方式。完整的可用选项可以在google/protobuf/descriptor.proto找到。

一些选项是文件级别的，意味着它可以作用于最外范围，不包含在任何消息内部、enum或服务定义中。一些选项是消息级别的，意味着它可以用在消息定义的内部。当然有些选项可以作用在域、enum类型、enum值、服务类型及服务方法中。到目前为止，并没有一种有效的选项能作用于所有的类型。

如下就是一些常用的选择：

* java\_package \(文件选项\) :这个选项表明生成java类所在的包。如果在.proto文件中没有明确的声明java\_package，就采用默认的包名。当然了，默认方式产生的 java包名并不是最好的方式，按照应用名称倒序方式进行排序的。如果不需要产生java代码，则该选项将不起任何作用。如：

```
option java_package = "com.example.foo";
```

* java\_outer\_classname \(文件选项\): 该选项表明想要生成Java类的名称。如果在.proto文件中没有明确的java\_outer\_classname定义，生成的class名称将会根据.proto文件的名称采用驼峰式的命名方式进行生成。如（foo\_bar.proto生成的java类名为FooBar.java）,如果不生成java代码，则该选项不起任何作用。如：

```
option java_outer_classname = "Ponycopter";
```

* optimize\_for\(文件选项\): 可以被设置为 SPEED, CODE\_SIZE,或者LITE\_RUNTIME。这些值将通过如下的方式影响C++及java代码的生成： 
  * SPEED \(default\): protocol buffer编译器将通过在消息类型上执行序列化、语法分析及其他通用的操作。这种代码是最优的。
  * CODE\_SIZE: protocol buffer编译器将会产生最少量的类，通过共享或基于反射的代码来实现序列化、语法分析及各种其它操作。采用该方式产生的代码将比SPEED要少得多， 但是操作要相对慢些。当然实现的类及其对外的API与SPEED模式都是一样的。这种方式经常用在一些包含大量的.proto文件而且并不盲目追求速度的 应用中。
  * LITE\_RUNTIME: protocol buffer编译器依赖于运行时核心类库来生成代码（即采用libprotobuf-lite 替代libprotobuf）。这种核心类库由于忽略了一 些描述符及反射，要比全类库小得多。这种模式经常在移动手机平台应用多一些。编译器采用该模式产生的方法实现与SPEED模式不相上下，产生的类通过实现 MessageLite接口，但它仅仅是Messager接口的一个子集。

```
option optimize_for = CODE_SIZE;
```

* cc\_enable\_arenas\(文件选项\):对于C++产生的代码启用[arena allocation](https://developers.google.com/protocol-buffers/docs/reference/arenas?hl=zh-cn)。
* objc\_class\_prefix\(文件选项\):设置Objective-C类的前缀，添加到所有Objective-C从此.proto文件产生的类和枚举类型。没有默认值，所使用的前缀应该是苹果推荐的3-5个大写字符，注意2个字节的前缀是苹果所保留的。
* deprecated\(字段选项\):如果设置为true则表示该字段已经被废弃，并且不应该在新的代码中使用。在大多数语言中没有实际的意义。在java中，这回变成@Deprecated注释，在未来，其他语言的代码生成器也许会在字标识符中产生废弃注释，废弃注释会在编译器尝试使用该字段时发出警告。如果字段没有被使用你也不希望有新用户使用它，尝试使用保留语句替换字段声明。

```
int32 old_field = 6 [deprecated=true];
```

### 自定义选项

ProtocolBuffers允许自定义并使用选项。该功能应该属于一个高级特性，对于大部分人是用不到的。如果你的确希望创建自己的选项，请参看[Proto2 Language Guide](https://developers.google.com/protocol-buffers/docs/proto?hl=zh-cn#customoptions)。注意创建自定义选项使用了拓展，拓展只在proto3中可用。

## 生成你的类

可以通过定义好的.proto文件来生成Java,Python,C++, Ruby, JavaNano, Objective-C,或者C\# 代码，需要基于.proto文件运行protocol buffer编译器protoc。如果你没有安装编译器，下载安装包并遵照README安装。对于Go,你还需要安装一个特殊的代码生成器插件。你可以通过GitHub上的protobuf库找到安装过程

通过如下方式调用protocol编译器：

```
protoc --proto_path=IMPORT_PATH --cpp_out=DST_DIR --java_out=DST_DIR --python_out=DST_DIR --go_out=DST_DIR --ruby_out=DST_DIR --javanano_out=DST_DIR --objc_out=DST_DIR --csharp_out=DST_DIR path/to/file.proto
```

* IMPORT\_PATH声明了一个.proto文件所在的解析import具体目录。如果忽略该值，则使用当前目录。如果有多个目录则可以多次调用--proto\_path，它们将会顺序的被访问并执行导入。-I=IMPORT\_PATH是--proto\_path的简化形式。
* 当然也可以提供一个或多个输出路径： 
  * --cpp\_out 在目标目录DST\_DIR中产生C++代码，可以在C++代码生成参考中查看更多。
  * --java\_out 在目标目录DST\_DIR中产生Java代码，可以在 Java代码生成参考中查看更多。
  * --python\_out 在目标目录 DST\_DIR 中产生Python代码，可以在Python代码生成参考中查看更多。
  * --go\_out 在目标目录 DST\_DIR 中产生Go代码，可以在GO代码生成参考中查看更多。
  * --ruby\_out在目标目录 DST\_DIR 中产生Go代码，参考正在制作中。
  * --javanano\_out在目标目录DST\_DIR中生成JavaNano，JavaNano代码生成器有一系列的选项用于定制自定义生成器的输出：你可以通过生成器的README查找更多信息，JavaNano参考正在制作中。
  * --objc\_out在目标目录DST\_DIR中产生Object代码，可以在Objective-C代码生成参考中查看更多。
  * --csharp\_out在目标目录DST\_DIR中产生Object代码，可以在C\#代码生成参考中查看更多。
  * --php\_out在目标目录DST\_DIR中产生Object代码，可以在PHP代码生成参考中查看更多。

作为一个方便的拓展，如果DST\_DIR以.zip或者.jar结尾，编译器会将输出写到一个ZIP格式文件或者符合JAR标准的.jar文件中。注意如果输出已经存在则会被覆盖，编译器还没有智能到可以追加文件。

* 你必须提议一个或多个.proto文件作为输入，多个.proto文件可以只指定一次。虽然文件路径是相对于当前目录的，每个文件必须位于其IMPORT\_PATH下，以便每个文件可以确定其规范的名称。


