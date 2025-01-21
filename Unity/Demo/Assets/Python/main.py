from audiometry import plot_audiograma, leer_datos
import matplotlib.pyplot as plt

# Llamada a la función para leer los datos del archivo
audiograma = leer_datos('Assets/Python/datos.txt')

# Dibujar la grafica
plot_audiograma(audiograma, colores=True)

# Guardar la imagen generada
plt.savefig('Assets/Python/audiogram.png')