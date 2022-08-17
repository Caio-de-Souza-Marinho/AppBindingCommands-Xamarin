using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AppBindingCommands
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Constructor
        public MainPageViewModel()
        {
            ShowMessageCommand = new Command(ShowMessage);
            CountCommand = new Command(async () => await CountCharacters());
            CleanCommand = new Command(async () => await CleanConfirmation());
            OptionCommand = new Command(async () => await ShowOptions());
        }

        public ICommand ShowMessageCommand { get; }
        public ICommand CountCommand { get; }
        public ICommand CleanCommand { get; }
        public ICommand OptionCommand { get; }

        #region Properties
        private string name = string.Empty;// CRTL, R+E para get e set

        public string Name
        {
            get => name; // get { return name; }
            set
            {
                if (name == null)
                    return;

                name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        public string DisplayName => $"Nome digitado: {Name}";

        string displayMessage = string.Empty;
        public string DisplayMessage
        {
            get => displayMessage;
            set
            {
                if (displayMessage == null)
                    return;

                displayMessage = value;
                OnPropertyChanged(nameof(DisplayMessage));
            }
        }
        #endregion

        #region Métodos
        public void ShowMessage()
        {
            string dataProperty = Application.Current.Properties["dtAtual"].ToString();

            DisplayMessage = $"Boa noite {Name}. Hoje é {dataProperty}";
        }

        public async Task CountCharacters()
        {
            string nameLength = string.Format("Seu nome tem {0} letras", name.Length);

            await Application.Current.MainPage.DisplayAlert("Informação", nameLength, "Ok");
        }

        public async Task CleanConfirmation()
        {
            if (await Application.Current.MainPage.DisplayAlert("Confirmação", "Confirma limpeza dos dados?", "Yes", "No"))
            {
                Name = string.Empty;
                DisplayMessage = string.Empty;
                OnPropertyChanged(Name);
                OnPropertyChanged(DisplayName);

                await Application.Current.MainPage.DisplayAlert("Informação", "Limpeza realizada com sucesso", "Ok");
            }
        }

        public async Task ShowOptions()
        {
            string result;

            result = await Application.Current.MainPage.DisplayActionSheet("Seleção", "selecione uma opção: ", "Cancelar", "Limpar", "Contar Caracteres", "Exibir Saudação");
        
            if (result == null)
            {
                if (result.Equals("Limpar"))
                    await CleanConfirmation();
                if(result.Equals("Contar caracteres"))
                    await CountCharacters();
                if (result.Equals("Exibir saudação"))
                    ShowMessage();
            }
        }
        #endregion
    }
}
