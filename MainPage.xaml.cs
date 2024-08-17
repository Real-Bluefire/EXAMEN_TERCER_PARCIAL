using Microsoft.Maui.Controls;
using EXAMEN_TERCER_PARCIAL.Models;
using EXAMEN_TERCER_PARCIAL.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using EXAMEN_TERCER_PARCIAL.Views;

namespace EXAMEN_TERCER_PARCIAL
{
    public partial class MainPage : ContentPage
    {
        private readonly FirebaseService _firebaseService = new FirebaseService();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnAnadirNotaClicked(object sender, EventArgs e)
        {
            var nota = new notes
            {
                NoteID = await GetNextId(),
                Desc = DescSelector.Text, // Actualiza el nombre del control
                Date = GetTimestamp(FechaSelector.Date), // Actualiza el nombre del control
            };

            await _firebaseService.AddNotaAsync(nota);

            await DisplayAlert("Guardado", "La nota se ha guardado exitosamente.", "Aceptar");
        }

        private async void OnTomarFotoClicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Selecciona una foto"
                });

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    PhotoImage.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
            }
        }

        private async void OnMostrarNotasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteList());
        }

        private async Task<int> GetNextId()
        {
            var notas = await _firebaseService.GetNotasAsync();
            return notas.Count > 0 ? notas.Max(n => n.NoteID) + 1 : 1;
        }

        private double GetTimestamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        private byte[] GetImageBytes(Image image)
        {
            if (image.Source is StreamImageSource streamImageSource)
            {
                using var stream = streamImageSource.Stream(CancellationToken.None).Result;
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
            return new byte[0];
        }
    }
}
