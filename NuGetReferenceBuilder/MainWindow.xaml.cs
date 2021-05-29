namespace NuGetReferenceBuilder
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Forms;
    using Annotations;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public bool IsFileLoaded { get; set; } = false;

        public List<string> RepositoriesTypes { get; } = new() {"Domain", "Application", "API"};
        public string RepositoriesType { get; set; } = "Domain";
        public string RootPath { get; set; } = "C:\\_dev\\";

        public string MasterBranchName { get; set; } = "master";
        public string FeatureBranchName { get; set; } = "FEAT-1234";
        public bool WillCommitChanges { get; set; } = true;
        public bool WillPushChanges { get; set; } = true;
        public bool WillUpdateSubmodules { get; set; } = true;
        public bool WillCreatePullRequest { get; set; } = true;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void ProjectPathSelector_OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.RootPath = folderBrowserDialog.SelectedPath;
                this.OnPropertyChanged(nameof(this.RootPath));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
