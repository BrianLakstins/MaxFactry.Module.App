rem Package the library for Nuget
copy ..\MaxFactry.Module.App-NF-4.5.2\bin\Release\MaxFactry.Module.App*.dll lib\App\net452\

c:\install\nuget\nuget.exe pack MaxFactry.Module.App.nuspec -OutputDirectory "packages" -IncludeReferencedProjects -properties Configuration=Release 

copy ..\MaxFactry.Module.App.Mvc4-NF-4.5.2\bin\MaxFactry.Module.App.Mvc4*.dll lib\App.Mvc4\net452\

c:\install\nuget\nuget.exe pack MaxFactry.Module.App.Mvc4.nuspec -OutputDirectory "packages" -IncludeReferencedProjects -properties Configuration=Release 
