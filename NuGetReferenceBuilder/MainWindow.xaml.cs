namespace NuGetReferenceBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Forms;
    using System.Xml;
    using Annotations;
    using Model;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public bool IsFileLoaded { get; set; }
        public ObservableCollection<NugetPackageReference> NugetPackageReferences { get; set; }
        public string ConsoleLogs { get; set; }

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
            this.NugetPackageReferences = new ObservableCollection<NugetPackageReference>();
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

        private void ResetFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var nugetPackageReference in this.NugetPackageReferences)
            {
                nugetPackageReference.TargetVersion = nugetPackageReference.CurrentVersion;
            }
        }

        private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
        {

            var doc = new XmlDocument();
            doc.Load("C:\\_dev\\WhiteLabel\\API\\NS.Booking.Booking.API\\src\\CustomLogic\\CustomLogic.csproj");
            var elemList = doc.GetElementsByTagName("PackageReference");
            for (int i=0; i < elemList.Count; i++)
            {
                var packageName = elemList[i].Attributes[0].InnerText;
                var packageVersion = elemList[i].InnerText;
                var nugetPackageReference = new NugetPackageReference
                {
                    Name = packageName,
                    CurrentVersion = packageVersion,
                    TargetVersion = packageVersion
                };
                this.NugetPackageReferences.Add(nugetPackageReference);
            }
        }
        
        private void RunScriptButton_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO: warn that unstaged changes in repos will be lost, and ask user confirmation
            
            var parameters = new Parameters
            {
                RepositoriesType = this.RepositoriesType,
                RootPath = this.RootPath,
                MasterBranchName = this.MasterBranchName,
                FeatureBranchName = this.FeatureBranchName,
                WillCommitChanges = this.WillCommitChanges,
                WillPushChanges = this.WillPushChanges,
                WillUpdateSubmodules = this.WillUpdateSubmodules,
                WillCreatePullRequest = this.WillCreatePullRequest
            };
            var runner = new PowershellScriptRunner();
            runner.Run(parameters, this.NugetPackageReferences);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
