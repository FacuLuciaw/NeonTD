using System;
using System.Collections.Generic;
using UnityEngine;

// Estructura principal para almacenar y gestionar los datos antes de enviarlos a la base de datos
[Serializable]
public class PartidaJSON
{
    public int id_user; 
    public string estado; 
    public string nivel;  
    public int dano_total_infligido;
    public int dano_total_recibido;
    public int oro_ganado;
    public int oro_gastado;
    public int rondas_completadas;

    [NonSerialized]
    private bool rondaContada;
    
    public List<DetalleTorre> torres = new List<DetalleTorre>();
    public List<DetalleEnemigo> enemigos = new List<DetalleEnemigo>();
    public List<string> enemigosActivos = new List<string>();

    // Registra una nueva torre construida o incrementa el contador si ya existe una del mismo tipo
    public void RegistrarTorre(string nombreTorre)
    {
        DetalleTorre torreExistente = torres.Find(t => t.nombre == nombreTorre);
        
        if (torreExistente != null)
        {
            torreExistente.cantidad++;
        }
        else
        {
            torres.Add(new DetalleTorre { nombre = nombreTorre, cantidad = 1 });
        }
    }

    // Registra un enemigo eliminado o incrementa su contador si ya se ha derrotado a uno igual
    public void RegistrarEnemigo(string nombreEnemigo)
    {
        DetalleEnemigo enemigoExistente = enemigos.Find(e => e.nombre == nombreEnemigo);
        
        if (enemigoExistente != null)
        {
            enemigoExistente.cantidad++;
        }
        else
        {
            enemigos.Add(new DetalleEnemigo { nombre = nombreEnemigo, cantidad = 1 });
        }
    }

    // Añade un enemigo a la lista de entidades presentes actualmente en el mapa
    public void AgregarEnemigoActivo(string nombreEnemigo)
    {
        if (!enemigosActivos.Contains(nombreEnemigo))
        {
            enemigosActivos.Add(nombreEnemigo);
        }
    }

    // Elimina a un enemigo de la lista de entidades activas al ser destruido o llegar a la base
    public void RemoverEnemigoActivo(string nombreEnemigo)
    {
        enemigosActivos.Remove(nombreEnemigo);
    }

    // Reduce el contador de una torre específica cuando el jugador la vende, o la elimina si solo quedaba una
    public void DesvincularTorre(string nombreTorre)
    {
        DetalleTorre torreExistente = torres.Find(t => t.nombre == nombreTorre);
        
        if (torreExistente != null)
        {
            if (torreExistente.cantidad > 1)
            {
                torreExistente.cantidad--;
            }
            else
            {
                torres.Remove(torreExistente);
            }
        }
    }

    // Suma la cantidad obtenida al registro total de oro de la partida
    public void RegistrarOroGanado(int cantidad)
    {
        oro_ganado += cantidad;
    }

    // Suma el coste de la torre al registro total de gastos de la partida al construir
    public void RegistrarOroGastado(int cantidad)
    {
        oro_gastado += cantidad;
    }

    // Resta una cantidad del total gastado al recibir un reembolso por venta
    public void RestarOroGastado(int cantidad)
    {
        oro_gastado -= cantidad;
        if (oro_gastado < 0) oro_gastado = 0;
    }

    // Acumula el daño total que la base principal ha sufrido durante la partida
    public void RegistrarDanoRecibido(float cantidad)
    {
        dano_total_recibido += (int)cantidad;
    }

    // Acumula el daño total que las torres han causado a los enemigos
    public void RegistrarDanoInfligido(float cantidad)
    {
        dano_total_infligido += (int)cantidad;
    }

    // Incrementa el contador de rondas superadas con éxito
    public void RegistrarRondaCompletada()
    {
        if (rondaContada)
        {
            return;
        }

        rondas_completadas++;
        rondaContada = true;
    }

    // Prepara el contador para una nueva ronda y evita contar múltiples veces la misma ronda
    public void IniciarNuevaRonda()
    {
        rondaContada = false;
    }

    // Actualiza el resultado de la partida (victoria, derrota, etc.) priorizando el estado infinito si está activo
    public void EstablecerEstado(string nuevoEstado)
    {
        if (estado != "infinito")
        {
            estado = nuevoEstado;
        }
    }

    // Guarda el nivel de dificultad en el que se está jugando
    public void EstablecerNivel(string nombreNivel)
    {
        nivel = nombreNivel;
    }
}

// Estructura auxiliar para guardar la cantidad construida de cada tipo de torre
[Serializable]
public class DetalleTorre 
{
    public string nombre;
    public int cantidad;
}

// Estructura auxiliar para guardar la cantidad eliminada de cada tipo de enemigo
[Serializable]
public class DetalleEnemigo 
{
    public string nombre;
    public int cantidad;
}