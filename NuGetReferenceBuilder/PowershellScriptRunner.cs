namespace NuGetReferenceBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Model;

    public class PowershellScriptRunner
    {
        string gitCommand = "git";

        string gitResetArgument = @"reset --hard";
        string gitCreateAndCheckoutToBranchArgument = @"checkout -b";
        string gitDeleteBranchArgument = @"checkout -D";
        string gitPullBranchArgument = @"pull origin";
        string gitCheckoutArgument = @"checkout -f";
        string gitSubmoduleUpdateArgument = @"submodule update --force";
        string gitAddArgument = @"add -A";
        string gitCommitArgument = @"commit -m";
        string gitPushArgument = @"push origin";
        
        public void Run(Parameters parameters, IEnumerable<NugetPackageReference> nugetPackageReferences)
        {
            // TODO: for each project inside "root" folder

            // remove unstaged changes
            Process.Start(this.gitCommand, this.gitResetArgument);

            // update master branch
            Process.Start(this.gitCommand, $"{this.gitCreateAndCheckoutToBranchArgument} auxiliar");
            Process.Start(this.gitCommand, $"{this.gitDeleteBranchArgument} {parameters.MasterBranchName}");
            Process.Start(this.gitCommand, $"{this.gitPullBranchArgument} {parameters.MasterBranchName}");
            Process.Start(this.gitCommand, $"{this.gitCheckoutArgument} {parameters.MasterBranchName}");
            Process.Start(this.gitCommand, $"{this.gitDeleteBranchArgument} auxiliar");

            // checkout to the feature branch
            Process.Start(this.gitCommand, $"{this.gitDeleteBranchArgument} {parameters.FeatureBranchName}");
            Process.Start(this.gitCommand, $"{this.gitCreateAndCheckoutToBranchArgument} {parameters.FeatureBranchName}");

            // update submodules
            Process.Start(this.gitCommand,
                $"{this.gitSubmoduleUpdateArgument} {(parameters.WillUpdateSubmodules ? "--force" : string.Empty)}");

            if (parameters.WillUpdateSubmodules)
            {
                Process.Start(this.gitCommand, this.gitAddArgument);
                if (parameters.WillCommitChanges)
                    Process.Start(this.gitCommand, $"{this.gitCommitArgument} updated submodules");
            }
            

            // TODO: stage submodule update changes, careful with submodule adds!
            // TODO: update .csproj and changelog here
            if (parameters.WillCommitChanges)
                Process.Start(this.gitCommand, $"{this.gitCommitArgument} updated nuget packages");
            
            if (parameters.WillPushChanges)
                Process.Start(this.gitCommand, $"{this.gitPushArgument} {parameters.FeatureBranchName}");

            if (parameters.WillCreatePullRequest)
            {
                // TODO: Using Azure API create PR and retrieve its link
            }

            //var process = new Process();
            //process.StartInfo.FileName = "powershell.exe";
            //process.StartInfo.Arguments = @"-executionpolicy unrestricted \\WIN-SRV-2019\Betreuung-Release\Install.ps1";

            //process.Start();
            //process.WaitForExit();
        }
    }
}
