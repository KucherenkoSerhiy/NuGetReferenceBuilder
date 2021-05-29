namespace NuGetReferenceBuilder.Model
{
    public class Parameters
    {
        public string RepositoriesType { get; set; }
        public string RootPath { get; set; }

        public string MasterBranchName { get; set; }
        public string FeatureBranchName { get; set; }
        public bool WillCommitChanges { get; set; }
        public bool WillPushChanges { get; set; }
        public bool WillUpdateSubmodules { get; set; }
        public bool WillCreatePullRequest { get; set; }
    }
}