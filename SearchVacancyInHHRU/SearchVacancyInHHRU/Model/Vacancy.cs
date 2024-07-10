using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SearchVacancyInHHRU.Model
{
    public class Vacancy : INotifyPropertyChanged
    {
        private string name;
        private string technologies;
        private string url;


        /// <summary>
        /// Конструктор для создания нового экземпляра вакансии.
        /// </summary>
        /// <param name="name">Название вакансии.</param>
        /// <param name="technologies">Технологии, связанные с вакансией.</param>
        /// <param name="url">URL-адрес, связанный с вакансией.</param>
        public Vacancy(string name, string technologies, string url)
        {
            this.name = name;
            this.technologies = technologies;
            this.url = url;
        }

        /// <summary>
        /// Наименование вакансии.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Технологии, связанные с вакансией.
        /// </summary>
        public string Technologies
        {
            get { return technologies; }
            set
            {
                technologies = value;
                OnPropertyChanged(nameof(Technologies));
            }
        }

        /// <summary>
        /// URL вакансии.
        /// </summary>
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        /// <summary>
        /// Событие, сигнализирующее об изменении свойства объекта.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод для вызова события PropertyChanged и уведомления об изменении свойства.
        /// </summary>
        /// <param name="prop">Имя измененного свойства.</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
