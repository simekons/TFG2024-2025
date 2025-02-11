import matplotlib
matplotlib.rcParams['ps.fonttype'] = 42
import matplotlib.pyplot as plt

def leer_datos(nombre_archivo):
    '''
    Genera un dict del audiograma mediante los datos del archivo
    Input:
        nombre_archivo [string] : path del archivo que leer.

    Output:
        datos [dict] : datos legibles para matplotlib y generar los vectores de la gráfica.
    '''
    
    datos = {'Left': {}, 'Right': {}}
    with open(nombre_archivo, 'r') as archivo:
        lado_actual = None
        for linea in archivo:
            linea = linea.strip()
            if linea == 'Left':
                lado_actual = 'Left'
            elif linea == 'Right':
                lado_actual = 'Right'
            else:
                frecuencia, valor = linea.split()
                datos[lado_actual][int(frecuencia)] = int(valor)
    return datos

def plot_colores():
    '''
    Separa la grafica en colores segun la gravedad de los datos.
    '''

    niveles = {'Normal': [-10,15,'#fdfefe'],
            'Leve': [15,25,'#d6eaf8'],
            'Suave': [25,40,'#85c1e9'],
            'Moderado': [40,55,'#5dade2'],
            'Moderadamente Severo': [55,70,'#3498d8'],
            'Severo': [70,90,'#2e86c1'],
            'Profundo': [90,130,'#2874a6']}

    for gravedad in niveles.keys():
        min_db = niveles[gravedad][0]
        max_db = niveles[gravedad][1]
        clr = niveles[gravedad][2]
        plt.fill_between(x=[100,10000], y1=[min_db, min_db], y2=[max_db, max_db], color=clr, alpha=1.0)
        plt.text(110, min_db + (max_db - min_db) / 2, gravedad, color='k', fontsize='small')

    plt.axhline(y=15, color='black', linestyle='-', linewidth=2)
    plt.axhline(y=25, color='black', linestyle='-', linewidth=2)
    plt.axhline(y=40, color='black', linestyle='-', linewidth=2)
    plt.axhline(y=55, color='black', linestyle='-', linewidth=2)
    plt.axhline(y=70, color='black', linestyle='-', linewidth=2)
    plt.axhline(y=90, color='black', linestyle='-', linewidth=2)

    plt.axvline(x=1000, color='black', linestyle='-', linewidth=2)

    plt.gca().spines['top'].set_linewidth(2)
    plt.gca().spines['bottom'].set_linewidth(2)
    plt.gca().spines['left'].set_linewidth(2)
    plt.gca().spines['right'].set_linewidth(2)

def plot_audiograma(audiograma, colores=False):
    '''
    Genera un grafico del audiograma mediante el audiograma del leer_data()
    Input:
        audiograma [dict] : genera esto mediante el leer_data(). Genera los vectores
                            de la gráfica.
        colores [bool] : True/False, pinta los colores de los distintos niveles:
                                Normal, Leve, Suave, Moderado, Moderadamente Severo,
                                Severo y Profundo.

    '''

    plt.figure()

    if colores:
        plot_colores()

    plt.plot(audiograma['Left'].keys(), audiograma['Left'].values(), 'x-', color='b', label='Izquierda')
    plt.plot(audiograma['Right'].keys(), audiograma['Right'].values(), 'o-', fillstyle='none', color='r', label='Derecha')
        
    plt.gca().set_xscale('log')
    plt.gca().axis([100, 10000, -10, 130])
    plt.gca().set_xticks([125,250,500,1000,2000,4000,8000])
    plt.gca().set_xticklabels([125,250,500,1000,2000,4000,8000])
    plt.grid()
    plt.ylabel('Volumen (dB)')
    plt.xlabel('Frecuencia (Hz)')
    plt.gca().invert_yaxis()
    plt.legend(loc='lower right')
    plt.title('Audiograma')