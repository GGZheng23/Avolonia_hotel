using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Avalonia_Hotel;

public partial class ViewModel:ObservableObject
{
    private GestioneDati gestione;
    private MainWindow mainWindow;
    private Calendar calendario;
    private List<DateTime> calendarioDiUnaCamera;
    private List<DateTime> dateSelezionate;
    [ObservableProperty] private Prenotazione prenotazioneSelezionataDataGrid;
    [ObservableProperty] private List<DettagliPrenotazione> listaDettagliPrenotazioni;
    [ObservableProperty] private List<Prenotazione> listaPrenotazioni;
    [ObservableProperty] private List<Camera> listaCamereDisponibili;
    [ObservableProperty] private ObservableCollection<Camera> listaCamereSelezionate;
    [ObservableProperty] private List<Camera> listaCamere;
    [ObservableProperty] private Camera cameraSelezionataComboBox;
    [ObservableProperty] private Camera cameraSelezionataDataGrid;
    [ObservableProperty] private Camera cameraSelezionataListBox;
    [ObservableProperty] private String nome;
    [ObservableProperty] private String cognome;
    [ObservableProperty] private String sesso;
    [ObservableProperty] private String telefono;
    [ObservableProperty] private String txtOutput;
    [ObservableProperty] private String txtOutput2;
    [ObservableProperty] private DateOnly dataCheckIn;
    [ObservableProperty] private DateOnly dataCheckOut;
    [ObservableProperty] private DateTime startDate;
    [ObservableProperty] private DateTime endDate;
    [ObservableProperty] private int numPersone;
    public ViewModel(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
        gestione = new GestioneDati();
        dateSelezionate = new List<DateTime>();
        ListaPrenotazioni = gestione.RecuperaTuttePrenotazioni();
        ListaCamere = gestione.RecuperaTutteCamere();
        ListaCamereSelezionate = new ObservableCollection<Camera>();
        this.mainWindow.Opened += OnWindowOpened;
    }

    [RelayCommand]
    public void AggiungiCamera(Camera camera)
    {
        ListaCamereSelezionate.Add(camera);
    }
    
    [RelayCommand]
    public void EliminaCamera(Camera camera)
    {
        ListaCamereSelezionate.Remove(camera);
    }
    [RelayCommand]
    public void Svuota()
    {
        ListaCamereSelezionate.Clear();
    }
    
    [RelayCommand]
    public void Conferma()
    {
        if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Cognome) ||
            string.IsNullOrWhiteSpace(Sesso) || string.IsNullOrWhiteSpace(Telefono) ||
            ListaCamereSelezionate.Count == 0) 
        {
            TxtOutput = "Compila tutti i campi!";
        }
        else
        {
            string dataIn = StartDate.Year + "-" + StartDate.Month + "-" + StartDate.Day;
            string dataOut = EndDate.Year + "-" + EndDate.Month + "-" + EndDate.Day;
            string num = NumPersone.ToString();
            TxtOutput=gestione.CrearePrenotazione(Nome, Cognome, Sesso, dataIn, dataOut, num, Telefono);
            string id = gestione.RecuperaIdDiUnaPrenotazione(Nome,Cognome);
            foreach (var camera in ListaCamereSelezionate)
            {
                TxtOutput = gestione.CreareDettagliPrenotazione(id, camera.id.ToString());
            }
            ListaPrenotazioni = gestione.RecuperaTuttePrenotazioni(); 
        }
        
    }
    [RelayCommand]
    public void Modifica()
    {

        if (PrenotazioneSelezionataDataGrid is null)
        {
            TxtOutput2 = "Seleziona una prenotazione!";
        }
        else
        {
            TxtOutput2=gestione.AggiornaPrenotazione(PrenotazioneSelezionataDataGrid.id.ToString(),
                                                               PrenotazioneSelezionataDataGrid.nomeCliente, 
                                                               PrenotazioneSelezionataDataGrid.cognomeCliente, 
                                                               PrenotazioneSelezionataDataGrid.sesso, 
                                                               PrenotazioneSelezionataDataGrid.numPersone.ToString(), 
                                                               PrenotazioneSelezionataDataGrid.telefono);
           
            ListaPrenotazioni = gestione.RecuperaTuttePrenotazioni();  
        }
        
        
    }
    [RelayCommand]
    public void Elimina()
    {
        if (PrenotazioneSelezionataDataGrid is null)
        {
            TxtOutput2 = "Seleziona una prenotazione!";
        }
        else
        {
            List<DettagliPrenotazione> listaDaEliminare=
                gestione.RecuperaTuttiDettagliDiUnaPrenotazione(PrenotazioneSelezionataDataGrid.id);
            foreach (var dp in listaDaEliminare)
            {
                TxtOutput2 = gestione.EliminaDettagliPrenotazione(dp.idPrenotazione.ToString(), dp.idCamera.ToString());
            }
            TxtOutput2=gestione.EliminaPrenotazione(PrenotazioneSelezionataDataGrid.id.ToString());
            ListaPrenotazioni = gestione.RecuperaTuttePrenotazioni(); 
        }
        
    }
    
    [RelayCommand]
    public void CellClick(object? sender)
    {
        if (PrenotazioneSelezionataDataGrid != null)
        {
            ListaDettagliPrenotazioni =
                gestione.RecuperaTuttiDettagliDiUnaPrenotazione(prenotazioneSelezionataDataGrid.id);
        }
        else
        {
            ListaDettagliPrenotazioni = null;
        }
    }
    
    private void OnWindowOpened(object? sender, EventArgs e)
    {
        // Aggiungi l'evento quando il Calendar è disponibile
        this.mainWindow.Calendar.PropertyChanged += OnCalendarPropertyChanged;
        this.mainWindow.Calendar2.SelectedDatesChanged += OnSelectedDatesChanged;
    }
    private void OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Aggiorna la lista di date selezionate
        var calendar = (Calendar)sender!;
        var dates = calendar.SelectedDates.OrderBy(d => d).ToList();
    
        // Calcola data di inizio e data di fine
        StartDate = dates.FirstOrDefault();
        EndDate = dates.LastOrDefault();

        if (StartDate!=null && EndDate!=null)
        {
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                dateSelezionate.Add(date);
            }
            HighlightDateRange(mainWindow.Calendar2, dateSelezionate);
            
            ListaCamereDisponibili = gestione.RecuperaTutteCamereDisponibili(dateSelezionate);
            dateSelezionate.Clear();
            ListaCamereSelezionate.Clear();
        }
    }
    
    private void OnCalendarPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // Rileva cambiamenti sulla visualizzazione del mese
        if (e.Property.Name == "DisplayDate" || e.Property.Name == "SelectedDate")
        {
            if (cameraSelezionataDataGrid != null)
            {
                calendarioDiUnaCamera = gestione.RecuperaCalendarioDiUnaCamera(cameraSelezionataDataGrid.id);
                HighlightDateRange(mainWindow.Calendar,calendarioDiUnaCamera);
            }
            
        }
    }
    private void HighlightDateRange(Calendar c, IEnumerable<DateTime> dates)
    {
        // Trova tutti i pulsanti dei giorni nel calendario
        var dayButtons = c.GetVisualDescendants()
            .OfType<Button>();

        // Colora i pulsanti che corrispondono all'intervallo
        foreach (var button in dayButtons)
        {
            if (button.DataContext is DateTime data)
            {
                if (dates.Contains(data))
                {
                    // Se la data è nell'intervallo, colora il pulsante di rosso
                    button.Background = Brushes.Red;
                }
                else if (data == DateTime.Today)
                {
                    // Se la data è oggi ma non è nell'intervallo, rendi trasparente
                    button.Background = Brushes.Transparent;
                }
                else
                {
                    // Altrimenti, colora il pulsante di verde
                    button.Background = Brushes.Transparent;
                }
            }
        }
    }

    
    
}