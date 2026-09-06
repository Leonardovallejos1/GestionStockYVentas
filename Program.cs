using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionStockYVentas
{
    // 1. CLASE PRODUCTO (Representa cada artículo)
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

    // 2. SISTEMA PRINCIPAL
    class Program
    {
        static List<Producto> inventario = new List<Producto>();
        static int contadorId = 1;

        static void Main(string[] args)
        {
            // Cargar datos iniciales para probar de entrada
            CargarDatosPrueba();

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("  SISTEMA DE CONTROL DE STOCK Y VENTAS  ");
                Console.WriteLine("========================================\n");
                Console.WriteLine("1. Ver Inventario");
                Console.WriteLine("2. Agregar Producto");
                Console.WriteLine("3. Realizar Venta");
                Console.WriteLine("4. Salir");
                Console.Write("\nSeleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarInventario();
                        break;
                    case "2":
                        AgregarProducto();
                        break;
                    case "3":
                        VenderProducto();
                        break;
                    case "4":
                        salir = true;
                        Console.WriteLine("\n¡Gracias por usar el sistema! Presione una tecla para salir...");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida. Presione cualquier tecla para reintentar.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Carga de productos iniciales para no arrancar de cero
        static void CargarDatosPrueba()
        {
            inventario.Add(new Producto(contadorId++, "Coca Cola 1.5L", 1500.00m, 10));
            inventario.Add(new Producto(contadorId++, "Galletitas", 850.50m, 5));
            inventario.Add(new Producto(contadorId++, "Agua Mineral 500ml", 600.00m, 2));
        }

        // Opción 1: Mostrar Inventario
        static void MostrarInventario()
        {
            Console.Clear();
            Console.WriteLine("--- INVENTARIO DE PRODUCTOS ---\n");

            if (inventario.Count == 0)
            {
                Console.WriteLine("No hay productos cargados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} | {"Nombre",-20} | {"Precio",-10} | {"Stock",-8}");
                Console.WriteLine(new string('-', 50));

                foreach (var prod in inventario)
                {
                    // Alerta visual de poco stock
                    string estadoStock = prod.Stock <= 2 ? $"{prod.Stock} (¡ALERTA!)" : prod.Stock.ToString();
                    Console.WriteLine($"{prod.Id,-5} | {prod.Nombre,-20} | ${prod.Precio,-9:F2} | {estadoStock,-8}");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
            Console.ReadKey();
        }

        // Opción 2: Agregar Producto
        static void AgregarProducto()
        {
            Console.Clear();
            Console.WriteLine("--- AGREGAR NUEVO PRODUCTO ---\n");

            Console.Write("Nombre del producto: ");
            string nombre = Console.ReadLine();

            Console.Write("Precio: $");
            if (!decimal.TryParse(Console.ReadLine(), out decimal precio) || precio <= 0)
            {
                Console.WriteLine("\n¡Error! Precio inválido.");
                Pausar();
                return;
            }

            Console.Write("Cantidad inicial de stock: ");
            if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
            {
                Console.WriteLine("\n¡Error! Cantidad de stock inválida.");
                Pausar();
                return;
            }

            inventario.Add(new Producto(contadorId++, nombre, precio, stock));
            Console.WriteLine("\n¡Producto guardado correctamente!");
            Pausar();
        }

        // Opción 3: Vender Producto (Descuenta Stock y calcula Total)
        static void VenderProducto()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR VENTA ---\n");

            Console.Write("Ingrese el ID del producto a vender: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n¡Error! ID inválido.");
                Pausar();
                return;
            }

            var producto = inventario.FirstOrDefault(p => p.Id == id);

            if (producto == null)
            {
                Console.WriteLine("\n¡Error! Producto no encontrado.");
                Pausar();
                return;
            }

            Console.WriteLine($"\nProducto seleccionado: {producto.Nombre}");
            Console.WriteLine($"Stock disponible: {producto.Stock}");
            Console.WriteLine($"Precio unitario: ${producto.Precio:F2}");

            Console.Write("\nIngrese la cantidad a vender: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad <= 0)
            {
                Console.WriteLine("\n¡Error! Cantidad no válida.");
                Pausar();
                return;
            }

            // VALIDACIÓN DE NEGOCIO: Verificar stock suficiente
            if (cantidad > producto.Stock)
            {
                Console.WriteLine($"\n¡ERROR DE VENTA! No hay suficiente stock. Solamente quedan {producto.Stock} unidades.");
            }
            else
            {
                // Descuento automático de stock
                producto.Stock -= cantidad;
                decimal total = cantidad * producto.Precio;

                Console.WriteLine("\n========================================");
                Console.WriteLine("           VENTA EXITOSA");
                Console.WriteLine("========================================");
                Console.WriteLine($"Cantidad vendida: {cantidad}");
                Console.WriteLine($"TOTAL A COBRAR: ${total:F2}");
                Console.WriteLine($"Nuevo stock de {producto.Nombre}: {producto.Stock}");
            }

            Pausar();
        }

        static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}