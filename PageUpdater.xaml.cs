using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using EXAMEN_TERCER_PARCIAL.Models;
using EXAMEN_TERCER_PARCIAL.Services;

namespace EXAMEN_TERCER_PARCIAL
{
    public partial class PageUpdater : ContentPage
    {
        private notes _nota;

        public PageUpdater(notes nota)
        {
            InitializeComponent();
            _nota = nota;
            PopulateFields();
        }

        private void PopulateFields()
        {
            DescSelector.Text = _nota.Desc;
            FechaSelector.Date = DateTimeOffset.FromUnixTimeSeconds((long)_nota.Date).DateTime;
        }


        private async void OnUpdateNotaClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescSelector.Text))
            {
                await DisplayAlert("Descripción Vacía", "Por favor ingresa una descripción para la nota.", "OK");
                return;
            }

            _nota.Desc = DescSelector.Text;
            _nota.Date = GetTimestamp(FechaSelector.Date);

            var firebaseService = new FirebaseService();
            await firebaseService.UpdateNotaAsync(_nota);

            await DisplayAlert("Actualización Exitosa", "La nota ha sido actualizada correctamente.", "OK");
            await Navigation.PopAsync();
        }

        private double GetTimestamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        private async Task<byte[]> GetImageBytes(Image image)
        {
            if (image.Source is StreamImageSource streamImageSource)
            {
                using var stream = await streamImageSource.Stream(CancellationToken.None);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
            return Array.Empty<byte>();
        }
    }
}
