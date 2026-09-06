using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace GestionStockWPF
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public Producto(int id, string nombre, decimal precio, int stock)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
        }
    }

    public partial class MainWindow : Window
    {
        List<Producto> inventario = new List<Producto>();
        int contadorId = 1;
        readonly string rutaArchivo = "inventario.txt";

        public MainWindow()
        {
            InitializeComponent();
            CargarDesdeArchivo();
            ActualizarTabla();
        }

        private void ActualizarTabla()
        {
            dgInventario.ItemsSource = null;
            dgInventario.ItemsSource = inventario;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                !decimal.TryParse(txtPrecio.Text, out decimal precio) ||
                !int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Por favor, ingrese datos válidos.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            inventario.Add(new Producto(contadorId++, txtNombre.Text, precio, stock));
            GuardarEnArchivo();
            ActualizarTabla();

            // Limpiar campos
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }

        private void GuardarEnArchivo()
        {
            try
            {
                List<string> lineas = inventario.Select(p => $"{p.Id};{p.Nombre};{p.Precio};{p.Stock}").ToList();
                File.WriteAllLines(rutaArchivo, lineas);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}");
            }
        }

        private void CargarDesdeArchivo()
        {
            if (File.Exists(rutaArchivo))
            {
                try
                {
                    string[] lineas = File.ReadAllLines(rutaArchivo);
                    inventario.Clear();
                    foreach (string linea in lineas)
                    {
                        string[] datos = linea.Split(';');
                        if (datos.Length == 4)
                        {
                            inventario.Add(new Producto(int.Parse(datos[0]), datos[1], decimal.Parse(datos[2]), int.Parse(datos[3])));
                        }
                    }
                    if (inventario.Count > 0) contadorId = inventario.Max(p => p.Id) + 1;
                }
                catch { inventario = new List<Producto>(); }
            }
        }
        // =========================================================
        // --- OPERACIONES SOBRE ELEMENTOS SELECCIONADOS EN LA TABLA ---        // =========================================================
        // =========================================================
        private void BtnVender_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventario.SelectedItem is Producto productoSeleccionado)
            {
                if (productoSeleccionado.Stock > 0)
                {
                    productoSeleccionado.Stock--;
                    GuardarEnArchivo();
                    ActualizarTabla();

                    MessageBox.Show($"Venta realizada. Nuevo stock de '{productoSeleccionado.Nombre}': {productoSeleccionado.Stock}", 
                                    "Venta Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Sin stock suficiente de '{productoSeleccionado.Nombre}'.", 
                                    "Error de Venta", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccioná un producto de la tabla.", 
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventario.SelectedItem is Producto productoSeleccionado)
            {
                var resultado = MessageBox.Show($"¿Seguro que querés eliminar '{productoSeleccionado.Nombre}' del sistema?",
                                                "Confirmar eliminación", 
                                                MessageBoxButton.YesNo, 
                                                MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    inventario.Remove(productoSeleccionado);
                    GuardarEnArchivo();
                    ActualizarTabla();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccioná un producto de la tabla.", 
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    } 
}
  