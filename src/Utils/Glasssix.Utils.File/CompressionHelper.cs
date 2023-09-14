﻿using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace Glasssix.Utils.File
{
    /// <summary>
    /// 解压压缩文件帮助类
    /// </summary>
    public static class CompressionHelper
    {
        /// <summary>
        /// 压缩文件成RAR或ZIP文件(需存在Winrar.exe(只要自己电脑上可以解压或压缩文件就存在Winrar.exe))
        /// </summary>
        /// <param name="filesPath">将要压缩的文件夹或文件的绝对路径</param>
        /// <param name="rarPathName">压缩后的压缩文件保存绝对路径（包括文件名称）</param>
        /// <param name="IsCover">所压缩文件是否会覆盖已有的压缩文件(如果不覆盖,所压缩文件和已存在的相同名称的压缩文件不会共同存在,只保留原已存在压缩文件)</param>
        /// <param name="PassWord">压缩密码(如果不需要密码则为空)</param>
        /// <returns>true(压缩成功);false(压缩失败)</returns>
        public static bool CondenseRarOrZip(string filesPath, string rarPathName, bool IsCover, string PassWord)
        {
            string rarPath = Path.GetDirectoryName(rarPathName);
            if (!Directory.Exists(rarPath))
                Directory.CreateDirectory(rarPath);
            using (Process Process1 = new Process())
            {
                Process1.StartInfo.FileName = "Winrar.exe";
                Process1.StartInfo.CreateNoWindow = true;
                string cmd = "";
                if (!string.IsNullOrEmpty(PassWord) && IsCover)
                    //压缩加密文件且覆盖已存在压缩文件( -p密码 -o+覆盖 )
                    cmd = string.Format(" a -ep1 -p{0} -o+ {1} {2} -r", PassWord, rarPathName, filesPath);
                else if (!string.IsNullOrEmpty(PassWord) && !IsCover)
                    //压缩加密文件且不覆盖已存在压缩文件( -p密码 -o-不覆盖 )
                    cmd = string.Format(" a -ep1 -p{0} -o- {1} {2} -r", PassWord, rarPathName, filesPath);
                else if (string.IsNullOrEmpty(PassWord) && IsCover)
                    //压缩且覆盖已存在压缩文件( -o+覆盖 )
                    cmd = string.Format(" a -ep1 -o+ {0} {1} -r", rarPathName, filesPath);
                else
                    //压缩且不覆盖已存在压缩文件( -o-不覆盖 )
                    cmd = string.Format(" a -ep1 -o- {0} {1} -r", rarPathName, filesPath);
                //命令
                Process1.StartInfo.Arguments = cmd;
                Process1.Start();
                Process1.WaitForExit();//无限期等待进程 winrar.exe 退出
                                       //Process1.ExitCode==0指正常执行，Process1.ExitCode==1则指不正常执行
                if (Process1.ExitCode == 0)
                {
                    Process1.Close();
                    return true;
                }
                else
                {
                    Process1.Close();
                    return false;
                }
            }
        }

        public static DataTable ExcelToDataTable(Stream fileStream, string file)
        {
            DataTable dt = new DataTable();
            IWorkbook Workbook = null;
            if (file == ".xlsx")
                Workbook = new XSSFWorkbook(fileStream);
            else if (file == ".xls")
                Workbook = new HSSFWorkbook(fileStream);

            if (Workbook != null)
            {
                try
                {
                    ISheet sheet = Workbook.GetSheetAt(0);
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                    //得到Excel工作表的行
                    IRow headerRow = sheet.GetRow(0);
                    //得到Excel工作表的总列数
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        dt.Columns.Add(cell.ToString());
                    }
                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        DataRow dataRow = dt.NewRow();

                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        dt.Rows.Add(dataRow);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ExcelToDataTable Error", ex);
                }
            }
            return dt;
        }

        public static DataTable ExcelToDataTable(string filePath)
        {
            DataTable dt = new DataTable();
            FileStream file = null;
            IWorkbook Workbook = null;
            try
            {
                using (file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (filePath.IndexOf(".xlsx") > 0)
                        Workbook = new XSSFWorkbook(file);
                    else if (filePath.IndexOf(".xls") > 0)
                        Workbook = new HSSFWorkbook(file);

                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0);
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                        //得到Excel工作表的行
                        IRow headerRow = sheet.GetRow(0);
                        //得到Excel工作表的总列数
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            ICell cell = headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }
                        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);

                            DataRow dataRow = dt.NewRow();

                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            dt.Rows.Add(dataRow);
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                if (file != null)
                {
                    file.Close();
                }
                Console.WriteLine("ExcelToDataTable Error", ex);
                return null;
            }
        }

        /// <summary>
        /// 文件流转table
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static DataTable ImportExccelStream(Stream fileStream, string file)
        {
            IWorkbook Workbook = null;
            try
            {
                if (file == ".xlsx")
                    Workbook = new XSSFWorkbook(fileStream);
                else if (file == ".xls")
                    Workbook = new HSSFWorkbook(fileStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //HSSFWorkbook hssfworkbook = new HSSFWorkbook(fileStream);
            fileStream.Dispose();
            ISheet sheet = Workbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            rows.MoveNext();
            HSSFRow row = (HSSFRow)rows.Current;
            for (int j = 0; j < sheet.GetRow(0).LastCellNum; j++)
            {
                //将第一列作为列表头
                dt.Columns.Add(row.GetCell(j).ToString());
            }
            while (rows.MoveNext())
            {
                row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable ReadExccel(string filePath)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                //File_Path = HostingEnvironment.MapPath(filePath);
                dt = ExcelToDataTable(filePath);
            }
            return dt;
        }

        /// <summary>
        /// 解压RAR和ZIP文件(需存在Winrar.exe(只要自己电脑上可以解压或压缩文件就存在Winrar.exe))
        /// </summary>
        /// <param name="UnPath">解压后文件保存目录</param>
        /// <param name="rarPathName">待解压文件存放绝对路径（包括文件名称）</param>
        /// <param name="IsCover">所解压的文件是否会覆盖已存在的文件(如果不覆盖,所解压出的文件和已存在的相同名称文件不会共同存在,只保留原已存在文件)</param>
        /// <param name="PassWord">解压密码(如果不需要密码则为空)</param>
        /// <returns>true(解压成功);false(解压失败)</returns>
        public static bool UnRarOrZip(string UnPath, string rarPathName, bool IsCover = true, string PassWord = null)
        {
            if (!Directory.Exists(UnPath))
                Directory.CreateDirectory(UnPath);
            using (Process Process1 = new Process())
            {
                Process1.StartInfo.FileName = "Winrar.exe";
                Process1.StartInfo.CreateNoWindow = true;
                string cmd = "";
                if (!string.IsNullOrEmpty(PassWord) && IsCover)
                    //解压加密文件且覆盖已存在文件( -p密码 )
                    cmd = string.Format(" x -p{0} -o+ {1} {2} -y", PassWord, rarPathName, UnPath);
                else if (!string.IsNullOrEmpty(PassWord) && !IsCover)
                    //解压加密文件且不覆盖已存在文件( -p密码 )
                    cmd = string.Format(" x -p{0} -o- {1} {2} -y", PassWord, rarPathName, UnPath);
                else if (IsCover)
                    //覆盖命令( x -o+ 代表覆盖已存在的文件)
                    cmd = string.Format(" x -o+ {0} {1} -y", rarPathName, UnPath);
                else
                    //不覆盖命令( x -o- 代表不覆盖已存在的文件)
                    cmd = string.Format(" x -o- {0} {1} -y", rarPathName, UnPath);
                //命令
                Process1.StartInfo.Arguments = cmd;
                Process1.Start();
                Process1.WaitForExit();//无限期等待进程 winrar.exe 退出
                                       //Process1.ExitCode==0指正常执行，Process1.ExitCode==1则指不正常执行
                if (Process1.ExitCode == 0)
                {
                    Process1.Close();
                    return true;
                }
                else
                {
                    Process1.Close();
                    return false;
                }
            }
        }
    }
}