require 'albacore'
current_dir = File.dirname(__FILE__)
mspec_runner_path = "#{current_dir}/packages/Machine.Specifications.0.5.8/tools/mspec-clr4.exe"
output_path   = "#{current_dir}/bin"
test_assembly = "StructureMap.AutoMocking.NSubstitute.Specs.dll"
test_assembly_path = "#{output_path}/#{test_assembly}"

task :default => [:build, :mspec]

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