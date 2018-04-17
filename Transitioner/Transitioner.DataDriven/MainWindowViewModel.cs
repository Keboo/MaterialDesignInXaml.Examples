using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Transitioner.DataDriven
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IList<Project> Projects { get; }

        private Project _SelectedProject;
        public Project SelectedProject
        {
            get => _SelectedProject;
            set => Set(ref _SelectedProject, value);
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get => _SelectedIndex;
            set => Set(ref _SelectedIndex, value);
        }

        public ICommand SelectProjectCommand { get; }

        public MainWindowViewModel()
        {
            Projects = new[]
            {
                new Project("Project 1")
                {
                    Files = new[]
                    {
                        new File("File 1"),
                        new File("File 2"),
                        new File("File 3"),
                        new File("File 4")
                    }
                },
                new Project("Project 2")
                {
                    Files = new[]
                    {
                        new File("File 1"),
                        new File("File 2"),
                        new File("File 3"),
                        new File("File 4")
                    }
                },
                new Project("Project 3")
                {
                    Files = new[]
                    {
                        new File("File 1"),
                        new File("File 2"),
                        new File("File 3"),
                        new File("File 4")
                    }
                },
            };

            SelectProjectCommand = new RelayCommand<Project>(OnSelectProject);
        }

        private void OnSelectProject(Project project)
        {
            SelectedProject = project;
            SelectedIndex = 1;
        }
    }

    public class Project
    {
        public Project(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IList<File> Files { get; set; }
    }

    public class File
    {
        public File(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}