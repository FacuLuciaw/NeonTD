using System;
using System.Collections.Generic;

[Serializable]
public class PartidaJSON
{
    public int id_user; // Lo obtendrías al hacer login
    public string estado; // 'victoria', 'derrota', 'rendicion'
    public string nivel;  // 'facil', 'normal', 'dificil'
    public int dano_total_infligido;
    public int dano_total_recibido;
    public int oro_ganado;
    public int oro_gastado;
    
    // Listas para las tablas intermedias (N:M)
    public List<DetalleTorre> torres = new List<DetalleTorre>();
    public List<DetalleEnemigo> enemigos = new List<DetalleEnemigo>();

    // Método para registrar una torre construida
    public void RegistrarTorre(string nombreTorre)
    {
        // Buscamos si ya existe una torre con ese nombre
        DetalleTorre torreExistente = torres.Find(t => t.nombre == nombreTorre);
        
        if (torreExistente != null)
        {
            // Si existe, aumentamos la cantidad
            torreExistente.cantidad++;
        }
        else
        {
            // Si no existe, la creamos
            torres.Add(new DetalleTorre { nombre = nombreTorre, cantidad = 1 });
        }
    }

    // Método para registrar un enemigo derrotado
    public void RegistrarEnemigo(string nombreEnemigo)
    {
        // Buscamos si ya existe un enemigo con ese nombre
        DetalleEnemigo enemigoExistente = enemigos.Find(e => e.nombre == nombreEnemigo);
        
        if (enemigoExistente != null)
        {
            // Si existe, aumentamos la cantidad
            enemigoExistente.cantidad++;
        }
        else
        {
            // Si no existe, lo creamos
            enemigos.Add(new DetalleEnemigo { nombre = nombreEnemigo, cantidad = 1 });
        }
    }
}

[Serializable]
public class DetalleTorre {
    public string nombre;
    public int cantidad;
}

[Serializable]
public class DetalleEnemigo {
    public string nombre;
    public int cantidad;
}