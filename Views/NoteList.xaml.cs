using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using EXAMEN_TERCER_PARCIAL.Models;
using EXAMEN_TERCER_PARCIAL.Services;

namespace EXAMEN_TERCER_PARCIAL.Views
{
    public partial class NoteList : ContentPage
    {
        private readonly FirebaseService _servicioFirebase;
        private ObservableCollection<notes> _listaDeNotas;

        public NoteList()
        {
            InitializeComponent();
            _servicioFirebase = new FirebaseService();
            _listaDeNotas = new ObservableCollection<notes>();
            notasListView.ItemsSource = _listaDeNotas;
        }

        private async void OnMostrarNotasClicked(object sender, EventArgs e)
        {
            try
            {
                var notas = await _servicioFirebase.LeerNotasAsync();
                _listaDeNotas.Clear();
                foreach (var nota in notas)
                {
                    _listaDeNotas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cargar las notas: {ex.Message}", "OK");
            }
        }

        private void NotaSeleccionada(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void EliminarNota(object sender, EventArgs e)
        {
            try
            {
                var itemDeslizado = sender as SwipeItem;
                var notaSeleccionada = itemDeslizado?.BindingContext as notes;

                if (notaSeleccionada != null)
                {
                    var confirmarEliminacion = await DisplayAlert("Confirmación", "¿Deseas eliminar esta nota permanentemente?", "Eliminar", "Cancelar");
                    if (confirmarEliminacion)
                    {
                        await _servicioFirebase.EliminarNotaAsync(notaSeleccionada.NoteID);
                        await DisplayAlert("Eliminación Exitosa", "La nota ha sido eliminada.", "Aceptar");
                        _listaDeNotas.Remove(notaSeleccionada);
                    }
                }
                else
                {
                    await DisplayAlert("Error", "La nota que deseas eliminar no está disponible.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }

        private async void ActualizarNota(object sender, EventArgs e)
        {
            try
            {
                var itemDeslizado = sender as SwipeItem;
                var notaSeleccionada = itemDeslizado?.BindingContext as notes;

                if (notaSeleccionada != null)
                {
                    await Navigation.PushAsync(new PageUpdater(notaSeleccionada));
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo recuperar la nota para actualizar.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }
}
