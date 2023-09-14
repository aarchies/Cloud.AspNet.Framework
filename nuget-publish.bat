::接收目标项目名称
SET targetName=%~1
::本地自定义存放nuget包的目录，需要修改为对于的目录
SET nugetPath=G:\desktop\back\glassix.aspnet.framework\nupkgs\
::判断项目文件夹是否存在，不存在则创建
if not exist %nugetPath%%targetName%  (md %nugetPath%%targetName%)
::打包项目并输出到指定目录（主要是为了解决版本号的问题，采用单独的文件夹存放包）
dotnet pack %targetName%.csproj -o %nugetPath%%targetName%
::发布项目文件夹下面的所有.nupkg包至服务器（由于没有办法获取到项目的版本号，
::所以无法准确的知道打包后的文件名是什么，所以直接推送整个文件夹下的包）
dotnet nuget push -s http://192.168.15.11:8888/v3/index.json %nugetPath%%targetName%\*.nupkg -k glasssixnuegtserver --skip-duplicate

