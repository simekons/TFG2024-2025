from audiometry import plot_audiograma, leer_datos
import matplotlib.pyplot as plt

# Llamada a la función para leer los datos del archivo
audiograma = leer_datos('datos.txt')

# Imprimir el diccionario generado
print(audiograma)

# Dibujar la grafica
plot_audiograma(audiograma, colores=True)

# Guardar la imagen generada
plt.savefig('audiogram.png')