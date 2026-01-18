/*
 * Este programa permite visualizar cómo ciertos algoritmos funcionan en la vida real. Se empieza con un número determinado
 * de barras con valores aleatorios. El usuario deberá elegir el algoritmo deseado pulsando las teclas:
 *
 * 1: Bubble Sort
 * 2: Insertion Sort
 * 3: Selection Sort
 * 4: Quick Sort
 *
 * Una vez el algoritmo se ha seleccionado, al pulsar espacio las barras comenzarán a ser organizadas según su tamaño, de
 * menor a mayor y de izquierda a derecha. Se mostrará el número de pasos realizados.
 */

using Raylib_cs;

namespace Algorithm_Visualizer;

class Program
{
    // Características de la ventana
    private const int AnchoVentana = 1000;
    private const int AltoVentana = 600;

    // Características de las barras
    private const int NumBarras = 100;
    private const int AnchoBarra = AnchoVentana / NumBarras;
    private const int AltoMin = 10;
    private const int AltoMax = AltoVentana - 50;

    private static int[] _valores = Generador(NumBarras, AltoMin, AltoMax);
    private static bool _ordenando; // Bool para comenzar los algoritmos
    private static int _paso = 1; // Contador de pasos
    private static int _contador; // Contador de elementos
    private static int _pasada; // Contador de pasadas
    private static string _algoritmo = "Ninguno"; // Algoritmo a utilizar
    private static int _numAlgoritmo; // Número del algoritmo a utilizar

    // Variables especiales Bubble Sort
    private static int _comparando1 = -1; // Índice de la primera barra comparando
    private static int _comparando2 = -1; // Índice de la segunda barra comparando

    // Variables especiales Selection Sort
    private static int _indiceMinimo;
    private static int _ultimoColocado = -1; // Índice del último elemento colocado en su posición final (solo para dibujar)

    // Variables especiales Insertion Sort
    // _contador
    // _pasada
    // _ultimoColocado

    // Generador de Arrays
    private static int[] Generador(int numBarras, int altoMin, int altoMax)
    {
        // Generación de array de valores aleatorios
        int[] valores = new int[numBarras];
        Random random = new Random();
        for (int i = 0; i < numBarras; i++)
        {
            valores[i] = random.Next(altoMin, altoMax);
        }
        return valores;
    }



    // Dibujar barras
    private static void Dibujar(int numBarras, int altoVentana, int anchoBarra)
    {
        switch (_numAlgoritmo)
        {
            case 0:
                for (int i = 0; i < numBarras; i++)
                {
                    int x = i * anchoBarra;
                    int y = altoVentana - _valores[i];
                    int alto = _valores[i];

                    Color color = Color.White;

                    Raylib.DrawRectangle(x, y, anchoBarra - 1, alto, color);
                }
                break;
            case 1:
                for (int i = 0; i < numBarras; i++)
                {
                    int x = i * anchoBarra;
                    int y = altoVentana - _valores[i];
                    int alto = _valores[i];

                    Color color = Color.White;

                    if (i == _comparando1 || i == _comparando2)
                        color = Color.Red; // Comparando
                    else if (i >= _valores.Length - _pasada)
                        color = Color.Green; // Ya ordenadas

                    Raylib.DrawRectangle(x, y, anchoBarra - 1, alto, color);
                }
                break;
            case 2:
                for (int i = 0; i < numBarras; i++)
                {
                    int x = i * anchoBarra;
                    int y = altoVentana - _valores[i];
                    int alto = _valores[i];

                    Color color = Color.White;

                    switch (i)
                    {
                        case var _ when i == _ultimoColocado:
                            color = Color.Green; // Último elemento colocado
                            break;
                        case var _ when i == _contador:
                            color = Color.Red; // Elemento que estamos comparando actualmente
                            break;
                        case var _ when i ==_indiceMinimo:
                            color = Color.Yellow; // Candidato a mínimo actual
                            break;
                        case var _ when (i < _pasada):
                            color = Color.Blue; // Ya ordenadas
                            break;
                        default:
                            break;
                    }

                    Raylib.DrawRectangle(x, y, anchoBarra - 1, alto, color);
                }
                break;
            case 3:
                for (int i = 0; i < numBarras; i++)
                {
                    int x = i * anchoBarra;
                    int y = altoVentana - _valores[i];
                    int alto = _valores[i];

                    Color color = Color.White;

                    switch (i)
                    {
                        case var _ when i == _ultimoColocado:
                            color = Color.Green; // Último elemento colocado
                            break;
                        case var _ when i == _contador + 1:
                            color = Color.Red; // Elemento que estamos insertando
                            break;
                        case var _ when i == _contador:
                            color = Color.Yellow; // Elemento con el que estamos comparando
                            break;
                        case var _ when (i < _pasada && i != _contador && i != _contador + 1 && i != _ultimoColocado):
                            color = Color.Blue; // Ya ordenadas
                            break;
                        default:
                            break;
                    }

                    Raylib.DrawRectangle(x, y, anchoBarra - 1, alto, color);
                }

                break;
        }
    }

    // Bubble Sort
    private static void BubbleSort()
    {
        _comparando1 = _contador;
        _comparando2 = _contador + 1;
        // Comparar elementos adyacentes
        if (_valores[_contador] > _valores[_contador + 1])
        {
            // Intercambiar
            (_valores[_contador], _valores[_contador + 1]) = (_valores[_contador + 1], _valores[_contador]);
        }

        _contador++;
        _paso++;

        // No había tenido en cuenta la pasada para que aquellos que están ordenados no los toque. Soy imbécil.
        // El if que viene a continuación está hecho por Claude.
        // Si llegamos al final de la pasada actual
        if (_contador < _valores.Length - 1 - _pasada) return;
        _contador = 0;
        _pasada++;

        // Tampoco había pensado en esto. Soy tonto :(
        // Si completamos todas las pasadas, detener
        if (_pasada < _valores.Length - 1) return;
        _ordenando = false;
        _contador = 0;
        _pasada = 0;
    }

    // Selection Sort
    private static void SelectionSort()
    {
        // Si es el inicio de una nueva pasada, inicializar variables
        if (_contador <= _pasada)
        {
            _indiceMinimo = _pasada;
            _contador = _pasada + 1;
        }

        // Comparar el elemento actual
        if (_contador < _valores.Length)
        {
            if (_valores[_indiceMinimo] > _valores[_contador])
            {
                _indiceMinimo = _contador;
            }
            _contador++;
            _paso++;
        }
        // Si terminamos de buscar el mínimo en esta pasada, hacer el intercambio
        else
        {
            (_valores[_pasada], _valores[_indiceMinimo]) = (_valores[_indiceMinimo], _valores[_pasada]);
            _ultimoColocado = _pasada;
            _pasada++;
            _contador = 0; // Resetear para que en el siguiente frame entre en el if de inicialización

            // Si completamos todas las pasadas, detener
            if (_pasada < _valores.Length - 1) return;
            _ordenando = false;
            _contador = 0;
            _pasada = 0;
            _ultimoColocado = -1;
        }
    }

    // Insertion Sort
    private static void InsertionSort()
    {
        // Si el contador es mayor o igual a cero y el valor en el índice contador es mayor que el de _contador +1,
        // intercambiamos valores y restamos uno a contador y sumamos uno al número de pasos
        if (_contador >= 0 && _valores[_contador] > _valores[_contador + 1])
        {
            (_valores[_contador], _valores[_contador + 1]) = (_valores[_contador + 1], _valores[_contador]);
            _contador--;
            _paso++;
        }
        // En el resto de casos el contador será menor que cero o el valor en el índice contador ya no sea mayor que
        // el de _contador +1. Entonces, aumentamos pasada y reiniciamos contador para la siguiente pasada y marcamos el
        // último colocado para pintarlo en verde luego
        else
        {
            _ultimoColocado = _contador + 1;
            _pasada++;
            _contador = _pasada - 1;

            // Condición para finalizar algoritmo
            if (_pasada >= _valores.Length)
            {
                _ordenando = false;
                _contador = 0;
                _pasada = 1;
                _ultimoColocado = -1;
            }
        }
    }

    static void Main()
    {
        // Creación de la ventana
        Raylib.InitWindow(AnchoVentana, AltoVentana, "Algoritmos de Ordenamiento");
        Raylib.SetTargetFPS(60);

        // Bucle principal
        while (!Raylib.WindowShouldClose())
        {
            // Tecla R para reiniciar
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                _valores = Generador(NumBarras, AltoMin, AltoMax);
                _ordenando = false;
                _paso = 1;
                _contador = 0;
                _pasada = 0;
            }

            // Tecla SPACE para comenzar
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                _ordenando = true;
            }

            // Detectar tecla pulsada
            int key = Raylib.GetKeyPressed();
            // Solo procesar si se presionó una tecla real (no Null)
            if (key != (int)KeyboardKey.Null)
            {
                switch ((KeyboardKey)key)
                {
                    case KeyboardKey.R:
                        _valores = Generador(NumBarras, AltoMin, AltoMax);
                        _algoritmo = "Ninguno";
                        _numAlgoritmo = 0;
                        _ordenando = false;
                        _paso = 1;
                        _contador = 0;
                        _pasada = 0;
                        break;
                    case KeyboardKey.Space:
                        if (_numAlgoritmo is > 0 and <= 4) _ordenando = true;
                        break;
                    case KeyboardKey.One:
                    case KeyboardKey.Kp1:
                        _algoritmo = "Bubble Sort";
                        _numAlgoritmo = 1;
                        _contador = 0;
                        _pasada = 0;
                        _paso = 1;
                        _ordenando = false;
                        break;
                    case KeyboardKey.Two:
                    case KeyboardKey.Kp2:
                        _algoritmo = "Selection Sort";
                        _numAlgoritmo = 2;
                        _contador = 0;
                        _pasada = 0;
                        _paso = 1;
                        _ordenando = false;
                        _indiceMinimo = 0;
                        _ultimoColocado = -1;
                        break;
                    case KeyboardKey.Three:
                    case KeyboardKey.Kp3:
                        _algoritmo = "Insertion Sort";
                        _numAlgoritmo = 3;
                        _contador = 0;
                        _pasada = 1;
                        _paso = 1;
                        _ordenando = false;
                        break;
                    case KeyboardKey.Four:
                    case KeyboardKey.Kp4:
                        _algoritmo = "Quick Sort";
                        _numAlgoritmo = 4;
                        _contador = 0;
                        _pasada = 0;
                        _paso = 1;
                        _ordenando = false;
                        break;
                    default:
                        _algoritmo = "Ninguno";
                        _numAlgoritmo = 0;
                        _contador = 0;
                        _pasada = 0;
                        _paso = 0;
                        _ordenando = false;
                        break;

                }
            }

            // Ordenamiento
            if (_ordenando && _numAlgoritmo > 0)
            {
                switch (_numAlgoritmo) // cambio a numeración, para evitar problemas con strings
                {
                    case 1:
                        BubbleSort();
                        break;
                    case 2:
                        SelectionSort();
                        break;
                    case 3:
                        InsertionSort();
                        break;
                    case 4:
                        //QuickSort();
                        break;
                    default:
                        break;
                }
            }

            // Dibujar
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Dibujar(NumBarras, AltoVentana, AnchoBarra);

            Raylib.DrawText($"SPACE: Ordenar | R: Reiniciar | Algoritmo: {_algoritmo} | Paso: {_paso}", 10, 10, 20, Color.White);
            Raylib.EndDrawing();

        }
        Raylib.CloseWindow();
    }
}