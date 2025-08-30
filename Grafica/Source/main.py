from audiometry import plot_audiograma, leer_datos
import matplotlib.pyplot as plt
import sys

# Llamada a la función para leer los datos del archivo
ruta_datos = sys.argv[1]
audiograma = leer_datos(ruta_datos + '/datos.txt')

# Dibujar la grafica
plot_audiograma(audiograma, colores=True)

# Guardar la imagen generada
plt.savefig(ruta_datos + '/audiogram.png')