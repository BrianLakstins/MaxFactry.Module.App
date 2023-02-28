rem Package the library for Nuget
copy ..\MaxFactry.Module.App-NF-2.0\bin\Release\MaxFactry.Module.App*.dll lib\App\net20\
copy ..\MaxFactry.Module.App-NF-4.5.2\bin\Release\MaxFactry.Module.App*.dll lib\App\net452\
copy ..\MaxFactry.Module.App-NF-4.7.2\bin\Release\MaxFactry.Module.App*.dll lib\App\net472\
copy ..\MaxFactry.Module.App-NF-4.8\bin\Release\MaxFactry.Module.App*.dll lib\App\net48\

c:\install\nuget\nuget.exe pack MaxFactry.Module.App.nuspec -OutputDirectory "packages" -IncludeReferencedProjects -properties Configuration=Release 

copy ..\MaxFactry.Module.App.Mvc4-NF-4.5.2\bin\MaxFactry.Module.App.Mvc4*.dll lib\App.Mvc4\net452\

c:\install\nuget\nuget.exe pack MaxFactry.Module.App.Mvc4.nuspec -OutputDirectory "packages" -IncludeReferencedProjects -properties Configuration=Release 

copy ..\MaxFactry.Module.App.Mvc5-NF-4.5.2\bin\MaxFactry.Module.App.Mvc5*.dll lib\App.Mvc5\net452\
copy ..\MaxFactry.Module.App.Mvc5-NF-4.7.2\bin\MaxFactry.Module.App.Mvc5*.dll lib\App.Mvc5\net472\
copy ..\MaxFactry.Module.App.Mvc5-NF-4.8\bin\MaxFactry.Module.App.Mvc5*.dll lib\App.Mvc5\net48\

c:\install\nuget\nuget.exe pack MaxFactry.Module.App.Mvc5.nuspec -OutputDirectory "packages" -IncludeReferencedProjects -properties Configuration=Release 
