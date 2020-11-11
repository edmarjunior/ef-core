using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCore.CursoEFCore;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // InserirDados();
            // ConsultarDados();
            // InserirCliente();
            // CadastrarPedido();
            // ConsultarPedidoCarregamentoAdiantado();
            // AtualizarDados();
            // AtualizarDadosDesconectados();
            // RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();


            /* de formas conectadas */

            // var cliente = db.Clientes.Find(2);
            // db.Clientes.Remove(cliente);
            // db.Remove(cliente);
            // db.Set<Cliente>().Remove(cliente);
            // db.Entry(cliente).State = EntityState.Deleted;

            /* de forma desconectada */
            db.Set<Cliente>().Remove(new Cliente { Id = 2 });

            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            cliente.Nome = "Cliente de teste Atualizado 2";
            // db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        private static void AtualizarDadosDesconectados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = new Cliente
            {
                Id = 1
            };

            db.Attach(cliente);

            db.Entry(cliente).CurrentValues.SetValues(new
            {
                Nome = "Cliente desconectado",
                Telefone = "7966669999"
            });

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db.Pedidos
                .Include(x => x.Itens)
                .ThenInclude(x => x.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            var produtos = db.Produtos
                .AsNoTracking()
                .Where(x => x.Ativo)
                .OrderBy(x => x.Id)
                .ToList();


            foreach (var produto in produtos)
            {
                Console.WriteLine("produto: " + produto.Id);
                db.Produtos.Find(produto.Id);
            }
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var db = new Data.ApplicationContext();

            // db.Produtos.Add(produto);
            // db.Set<Produto>().Add(produto);
            // db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine("Registros afetados: " + registros);
        }

        private static void InserirCliente()
        {
            var cliente = new Cliente
            {
                Nome = "Tábata",
                Email = "edmar@gmail.com",
                CEP = "14404031",
                Cidade = "FRANCA",
                Estado = "SP",
                Telefone = "16992843688"
            };

            var db = new Data.ApplicationContext();

            db.Add(cliente);

            var registros = db.SaveChanges();

            Console.WriteLine("Clientes salvos: " + registros);
        }
    }
}
