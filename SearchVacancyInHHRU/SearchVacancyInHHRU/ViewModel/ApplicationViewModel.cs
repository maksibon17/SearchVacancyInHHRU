using Newtonsoft.Json.Linq;
using SearchVacancyInHHRU.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace SearchVacancyInHHRU.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Коллекция всех вакансий.
        /// </summary>
        public ObservableCollection<Vacancy> allVacancies { get; set; }

        /// <summary>
        /// Выбранная вакансия.
        /// </summary>
        private Vacancy selectedVacancy;

        /// <summary>
        /// Свойство для доступа к выбранной вакансии.
        /// </summary>
        public Vacancy SelectedVacancy
        {
            get { return selectedVacancy; }
            set
            {
                // Установка значения выбранной вакансии.
                selectedVacancy = value;
                // Уведомление об изменении свойства "SelectedVacancy".
                OnPropertyChanged(nameof(SelectedVacancy));
            }
        }

        /// <summary>
        /// Коллекция вакансий, соответствующих критериям поиска.
        /// </summary>
        private ObservableCollection<Vacancy> matchingVacancies;

        /// <summary>
        /// Свойство для доступа к коллекции вакансий, соответствующих критериям поиска.
        /// </summary>
        public ObservableCollection<Vacancy> MatchingVacancies
        {
            get { return matchingVacancies; }
            set
            {
                // Установка значения коллекции вакансий, соответствующих критериям поиска.
                matchingVacancies = value;

                // Уведомление об изменении свойства "MatchingVacancies".
                OnPropertyChanged(nameof(MatchingVacancies));
            }
        }

        /// <summary>
        /// Вводимый пользователем текст.
        /// </summary>
        private string textInput;

        /// <summary>
        /// Свойство для доступа к вводимому пользователем тексту.
        /// </summary>
        public string TextInput
        {
            get { return textInput; }
            set
            {
                // Установка значения вводимого пользователем текста.
                textInput = value;

                // Уведомление об изменении свойства "TextInput".
                OnPropertyChanged(nameof(TextInput));
            }
        }

        /// <summary>
        /// Текст для метки, отображающей вводимый текст.
        /// </summary>
        private string labelForInputText;

        /// <summary>
        /// Свойство для доступа к тексту для метки, отображающей вводимый текст.
        /// </summary>
        public string LabelForInputText
        {
            get { return labelForInputText; }
            set
            {
                // Установка значения текста для метки.
                labelForInputText = value;

                // Уведомление об изменении свойства "LabelForInputText".
                OnPropertyChanged(nameof(LabelForInputText));
            }
        }

        /// <summary>
        /// Текст для метки, отображающей количество найденных вакансий.
        /// </summary>
        private string labelForCountFoundVacancies;

        /// <summary>
        /// Свойство для доступа к тексту для метки, отображающей количество найденных вакансий.
        /// </summary>
        public string LabelForCountFoundVacancies
        {
            get { return labelForCountFoundVacancies; }
            set
            {
                // Установка значения текста для метки.
                labelForCountFoundVacancies = value;

                // Уведомление об изменении свойства "LabelForCountFoundVacancies".
                OnPropertyChanged(nameof(LabelForCountFoundVacancies));
            }
        }

        private string labelHowToOpenVacancy;

        /// <summary>
        /// Свойство для доступа к тексту для метки, выводящая подсказку, как перейти к вакансии.
        /// </summary>
        public string LabelHowToOpenVacancy
        {
            get { return labelHowToOpenVacancy; }
            set
            {
                labelHowToOpenVacancy = value;
                OnPropertyChanged(nameof(LabelHowToOpenVacancy));
            }
        }

        private string labelForEmptyList;

        /// <summary>
        /// Свойство для доступа к тексту для метки, отображающей текст об отсутствии найденных вакансий по указанной технологии.
        /// </summary>
        public string LabelForEmptyList
        {
            get { return labelForEmptyList; }
            set
            {
                labelForEmptyList = value;
                OnPropertyChanged(nameof(LabelForEmptyList));
            }
        }

        /// <summary>
        /// Получает команду для переключения видимости кнопки.
        /// </summary>
        public ICommand ToggleButtonVisibilityCommand { get; }

        private bool isButtonVisible;

        /// <summary>
        /// Свойство для доступа к логическому полю, которое получает или устанавливает значение, указывающее, видима ли кнопка.
        /// </summary>
        public bool IsButtonVisible
        {
            get => isButtonVisible;
            set
            {
                if (isButtonVisible != value)
                {
                    isButtonVisible = value;
                    OnPropertyChanged(nameof(IsButtonVisible));
                }
            }
        }

        /// <summary>
        /// Команда, которая реализует поиск вакансий по указанной пользователем технологии
        /// </summary>
        private Command searchCommand;

        /// <summary>
        /// Свойство комманды поиска вакансий.
        /// </summary>
        public Command SearchCommand
        {
            get
            {
                return searchCommand ?? (searchCommand = new Command(obj =>
                {
                    string searchTechnology = TextInput;
                    if (!string.IsNullOrWhiteSpace(searchTechnology))
                    {
                        // Удаляем лишние пробелы из текста.
                        searchTechnology = Regex.Replace(TextInput, @"\s+", " ");

                        // Выполняем поиск вакансий по указанной технологии и добавляем их в контейнер.
                        MatchingVacancies = SearchVacanciesByTechnology(searchTechnology.ToLower());

                        LabelForInputText = "Указанная технология: " + searchTechnology;
                        LabelForCountFoundVacancies = "Кол-во вакансий с этой технологией: " + MatchingVacancies.Count.ToString();
                        if (MatchingVacancies.Count > 0)
                        {
                            LabelForEmptyList = "";
                            LabelHowToOpenVacancy = "Чтобы перейти к самой ваканссии, выделите и нажмите кнопку 'Перейти к вакансии'";
                            IsButtonVisible = true;
                            
                        }
                        else
                        {
                            LabelForEmptyList = "Список вакансий пуст";
                            LabelHowToOpenVacancy = "";
                            IsButtonVisible = false;
                            
                        }
                        
                    }
                    else
                    {
                        ShowErrorMessage.Execute("Введите в строку поиска искомую технологию!\nСтрока не должна быть пустой!");
                        TextInput = string.Empty; // очищаем строку от лишних пробелов
                    }
                }));
            }
        }

        /// <summary>
        /// Команда открытия (на сайте hh.ru) выбранной вакансии.
        /// </summary>
        private Command openVacancyCommand;

        /// <summary>
        /// Свойство команды открытия (на сайте hh.ru) вакансии.
        /// </summary>
        public Command OpenVacancyCommand
        {
            get
            {
                return openVacancyCommand ?? (openVacancyCommand = new Command(obj =>
                {
                    if (SelectedVacancy != null)
                    {
                        string searchTechnology = TextInput.Replace(' ', '+');
                        var vacancyForOpen = SelectedVacancy;

                        // Открываем вакансию в браузере, добавляя в запрос строку для поиска технологии.
                        Device.OpenUri(new Uri(vacancyForOpen.Url + "?query=" + searchTechnology));
                    }
                    else ShowErrorMessage.Execute("Чтобы открыть вакансию, нужно её сперва выбрать!\nВыберите вакансию!");
                }));
            }
        }

        /// <summary>
        /// Команда вывода ошибочного сообщения на экран.
        /// </summary>
        private Command showErrorMessage;

        /// <summary>
        /// Свойство команды вывода ошибочного сообщения на экран.
        /// </summary>
        public Command ShowErrorMessage
        {
            get
            {
                return showErrorMessage ?? (showErrorMessage = new Command(async obj =>
                {
                    string errorMessage = obj as string;
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", errorMessage, "OK");
                    }
                }));
            }
        }

        /// <summary>
        /// Команда выхода из приложения.
        /// </summary>
        private Command exitingInApplication;

        /// <summary>
        /// Свойтсво команды выхода из приложения.
        /// </summary>
        public Command ExitingInApplication
        {
            get
            {
                return exitingInApplication ?? 
                    (exitingInApplication = new Command(obj =>
                {
                    if (obj is int exitCode)
                    {
                        // В Xamarin.Forms нет прямого метода для выхода из приложения, но можно использовать зависимости платформы.
                        DependencyService.Get<IAppCloser>()?.CloseApp();
                    }
                }));
            }
        }

        /// <summary>
        /// Конструктор класса ApplicationViewModel, инициализирующий объект ViewModel.
        /// </summary>
        public ApplicationViewModel()
        {
            const int COUNT_PAGE_WITH_VACANCY = 3; // на 1 странице - 100 вакансий
            allVacancies = new ObservableCollection<Vacancy>();

            for (int numberPage = 0; numberPage < COUNT_PAGE_WITH_VACANCY; numberPage++)
            {
                dynamic objJSON = GettingJSONObjectFromWebsiteHHRU(numberPage);
                AddAllVacanciesInObservableCollection(objJSON);
            }
        }

        /// <summary>
        /// Получение JSON-объекта с вакансиями с веб-сайта hh.ru. 
        /// </summary>
        /// <param name="numberPage">Номер страницы с вакансиями.</param>
        /// <returns>Динамический объект JSON с данными о вакансиях.</returns>
        private dynamic GettingJSONObjectFromWebsiteHHRU(int numberPage)
        {
            string temp = "";
            dynamic objJSON = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Chrome");
                    string url = $"https://api.hh.ru/vacancies?per_page=100&page={numberPage}";
                    temp = client.DownloadString(url);
                    objJSON = JObject.Parse(temp);
                }
            }
            catch (Exception)
            {
                ShowErrorMessage.Execute("Проверьте подключение к интернету");
                ExitingInApplication.Execute(1);
            }
            return objJSON;
        }

        /// <summary>
        /// Добавляет все вакансии из объекта JSON в коллекцию allVacancies.
        /// </summary>
        /// <param name="objectJSON">Объект JSON, содержащий информацию о вакансиях.</param>
        private void AddAllVacanciesInObservableCollection(dynamic objectJSON)
        {
            try
            {
                foreach (dynamic item in objectJSON.items)
                {
                    string technologies = item.snippet.requirement;
                    if (!string.IsNullOrEmpty(technologies))
                    {
                        string name = item.name;
                        string url = item.alternate_url;
                        Vacancy vacancy = new Vacancy(name, technologies.ToLower(), url);
                        allVacancies.Add(vacancy);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage.Execute(ex.Message);
                ExitingInApplication.Execute(1);
            }
        }

        /// <summary>
        /// Метод, реализующий поиск вакансий по указанной пользователем технологии среди всех вакансий из списка вакансий allVacancies
        /// </summary>
        /// <param name="searchText">Технология, введёная пользователем в строку поиска, по которой ищем вакансии</param>
        /// <returns>Коллекция вакансий, соответствующая критериям поиска</returns>
        private ObservableCollection<Vacancy> SearchVacanciesByTechnology(string searchText)
        {
            ObservableCollection<Vacancy> matchingVacancies = new ObservableCollection<Vacancy>();
            string[] wordsInSearchText = searchText.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var vacancy in allVacancies)
            {
                if (wordsInSearchText.All(word => vacancy.Technologies.Contains(word)))
                {
                    matchingVacancies.Add(vacancy);
                }
            }
            return matchingVacancies;
        }

        /// <summary>
        /// Событие, сигнализирующее об изменении свойства объекта.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод для вызова события PropertyChanged и уведомления об изменении свойства.
        /// </summary>
        /// <param name="prop">Имя измененного свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Интерфейс для закрытия приложения.
    /// </summary>
    public interface IAppCloser
    {
        void CloseApp();
    }
}
