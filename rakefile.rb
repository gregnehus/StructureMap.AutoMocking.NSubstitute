require 'albacore'

product_name = "StructureMap.AutoMocking.NSubstitute"
version = "2.0.0"
current_dir = File.dirname(__FILE__)
mspec_runner_path = "#{current_dir}/packages/Machine.Specifications.0.5.8/tools/mspec-clr4.exe"
nuget_path = "#{current_dir}/packages/NuGet.CommandLine.2.0.40001/tools/NuGet.exe"
output_path   = "#{current_dir}/bin"
test_assembly = "StructureMap.AutoMocking.NSubstitute.Specs.dll"
test_assembly_path = "#{output_path}/#{test_assembly}"

task :default => [:assemblyinfo,:build, :mspec, :createNugetPackage]

assemblyinfo :assemblyinfo do |asm|
  asm.version = version
  asm.product_name = product_name
  asm.description = "This package gives NSubstitute support for AutoMocking with StructureMap"
  asm.copyright = "Greg Nehus Copyright 2012"
  asm.output_file = "#{output_path}/AssemblyInfo.cs"
end

msbuild :build do |bld|
	bld.properties = {
		:configuration => :Release,
		:outputpath => output_path
	}
	bld.verbosity = 'minimal'
	bld.targets :Clean, :Build
	bld.solution = "StructureMap.AutoMocking.nSubstitute.sln"
end

mspec :mspec => [:build] do |mspec|
	mspec.command = mspec_runner_path
	mspec.assemblies = [test_assembly_path]
end

exec :createNugetPackage do |cmd|
	cmd.command = nuget_path
	cmd.parameters = "pack #{product_name}.nuspec -version #{version} -nodefaultexcludes -outputdirectory #{output_path} -basepath #{output_path}"
end
