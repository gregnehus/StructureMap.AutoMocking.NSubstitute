require 'albacore'
require 'version_bumper'

version = bumper_version


product_name = "StructureMap.AutoMocking.NSubstitute"

current_dir = File.dirname(__FILE__)
mspec_runner_path = "#{current_dir}/packages/Machine.Specifications.0.5.8/tools/mspec-clr4.exe"
nuget_path = "#{current_dir}/packages/NuGet.CommandLine.2.0.40001/tools/NuGet.exe"
output_path   = "#{current_dir}/bin"
properties_path = "#{current_dir}/StructureMap.AutoMocking.nSubstitute/Properties"
assembly_path = "#{properties_path}/AssemblyInfo.cs"
test_assembly = "StructureMap.AutoMocking.NSubstitute.Specs.dll"
test_assembly_path = "#{output_path}/#{test_assembly}"

task :default => [:build, :mspec, :createNugetPackage]

task :bumpVersion do
	version.bump_build
	version.write('VERSION')
end

assemblyinfo :createAssembly => :bumpVersion do |asm|
  build_version = version.to_s
  asm.title = product_name
  asm.version = build_version
  asm.file_version = build_version
  asm.company_name = "Greg Nehus"
  asm.product_name = product_name
  asm.description = "This package gives NSubstitute support for AutoMocking with StructureMap"
  asm.output_file = assembly_path
end

msbuild :build => :createAssembly do |bld|
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
	cmd.parameters = "pack #{product_name}.nuspec -version #{version.to_s} -nodefaultexcludes -outputdirectory #{output_path} -basepath #{output_path}"
end
