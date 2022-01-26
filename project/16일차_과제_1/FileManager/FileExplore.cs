using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FileManager
{
    public class ExploreBase
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class ExploreItems : List<ExploreBase> { }

    public class FolderItem : ExploreBase
    {
        public ExploreItems SubItems { get; set; }
    }

    public class FileItem : ExploreBase { }


    public class FileExplore
    {
        public static FolderItem SearchAll(FolderItem baseItem)
        {
            try
            {
                var path = baseItem.Path;

                // 서브폴더 검색
                var dirs = Directory.GetDirectories(path);
                ExploreItems subItems = new ExploreItems();
                if (dirs != null && dirs.Count() > 0)
                {
                    foreach (var dir in dirs)
                    {
                        var directory = new DirectoryInfo(dir);
                        if ((directory.Attributes & FileAttributes.System) == 0  &&
                            (directory.Attributes & FileAttributes.Hidden) == 0)
                        {
                            var name = dir.Split('\\').LastOrDefault();
                            var subItem = new FolderItem() { Name = name, Path = dir };
                            subItem = SearchAll(subItem);
                            subItems.Add(subItem);
                        }
                    }
                }

                // 파일 검색
                var files = Directory.GetFiles(path);
                if (files != null && files.Count() > 0)
                {
                    foreach (var file in files)
                    {
                        var name = Path.GetFileName(file);
                        subItems.Add(new FileItem() { Name = name, Path = file });
                    }
                }

                // 서브 아이템이 있다면
                if (subItems != null && subItems.Count() > 0 )
                    baseItem.SubItems = subItems;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        
            return baseItem;
        }


        public static FolderItem SearchFolders(FolderItem baseItem)
        {
            try
            {
                var path = baseItem.Path;

                // 서브폴더 검색
                var dirs = Directory.GetDirectories(path);
                ExploreItems subItems = new ExploreItems();
                if (dirs != null && dirs.Count() > 0)
                {
                    foreach (var dir in dirs)
                    {
                        var directory = new DirectoryInfo(dir);
                        if ((directory.Attributes & FileAttributes.System) == 0 &&
                            (directory.Attributes & FileAttributes.Hidden) == 0)
                        {
                            var name = dir.Split('\\').LastOrDefault();
                            var subItem = new FolderItem() { Name = name, Path = dir };
                            subItem = SearchFolders(subItem);
                            subItems.Add(subItem);
                        }
                    }
                }

                // 서브 아이템이 있다면
                if (subItems != null && subItems.Count() > 0)
                    baseItem.SubItems = subItems;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return baseItem;
        }


        public static FolderItem SearchFiles(FolderItem baseItem)
        {
            try
            {
                var path = baseItem.Path;

                // 파일 검색
                ExploreItems subItems = new ExploreItems();
                var files = Directory.GetFiles(path);
                if (files != null && files.Count() > 0)
                {
                    foreach (var file in files)
                    {
                        var attrib = File.GetAttributes(file);
                        if ((attrib & FileAttributes.Hidden) == 0 && 
                            (attrib & FileAttributes.System) == 0 )
                        {
                            var name = Path.GetFileName(file);
                            subItems.Add(new FileItem() { Name = name, Path = file });
                        }
                    }
                }

                // 서브 아이템이 있다면
                if (subItems != null && subItems.Count() > 0)
                    baseItem.SubItems = subItems;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return baseItem;
        }
    }

}
