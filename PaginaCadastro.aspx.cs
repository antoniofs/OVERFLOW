using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.View
{
    public partial class PaginaCadastro : System.Web.UI.Page
    {
        
        #region Declarações de Variaveis
        protected Boolean invalido = true;
        protected ProjectContext bd;
        #endregion

        //PageLoard da pagina
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            funcaoJS();
            if(!Page.IsPostBack)
            {
                bd = new ProjectContext();

                var uf = (from estados in bd.ESTADOS select estados.NM_SIGLA).ToList();
                DropDownUf.DataSource = uf;
                DropDownUf.DataBind();

                string UF = DropDownUf.SelectedItem.ToString();

                var municipios = (from mun in bd.MUNICIPIOs where mun.UF == UF select mun.NOME).ToList();

                DropDownCidade.DataSource = municipios;
                DropDownCidade.DataBind();


            }

        }
        #endregion

        //Evento click do botão cadastrar
        //Retorna 'True' se o cadastro foi realizado com sucesso ou 'False' se alguma coisa estiver errada
        #region Evento Click Do Botao
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            
            
            try
            {
              bd = new ProjectContext();
              validarDados();
              if(invalido == false)
              {
                  if(cadastrarNovoUsuario() == true)
                  {
                      ClientScript.RegisterClientScriptBlock(Page.GetType(), "Confirmacao", "alert('Usuario Cadastrado com Sucesso')", true);
                  }
                  else
                  {
                      ClientScript.RegisterClientScriptBlock(Page.GetType(), "Erro", "Alert('Ocorreu um erro na hora de gerar seu cadastro tente mais tarde (ERRO 40)')", true);
                  }
              }
            }catch(Exception ex)
            {
                ex.Message.Count();
            }
        }
        #endregion

        //Caso de teste caso exista um usuario com o mesmo usuario
        //Recebe como parametro o nome do Usuario do TxtUsuario
        #region Validação Existe Esse Usuario?
        private bool ExistenciaDeUsuario(string nomeUsuario)
        {
            bd = new ProjectContext();
            CLIENTE cliente = new CLIENTE();
            cliente = bd.CLIENTEs.Where(client => client.USUARIO == nomeUsuario).FirstOrDefault();

            if (cliente == null){
                Label16.Text = "";
                return true;
            }else
            {
                Label16.Text = "USUARIO CADASTRADO, TENTE OUTRO";
                return false;
            }
        }
        #endregion

        //Caso de teste para validar os dados inseridos pelo usuario
        //Recebe como parametro void
        #region Validação dos Usuarios
        protected void validarDados()
        {
            try
            {
                if (txtNome.Text == string.Empty) { Label8.Text = "CAMPO OBRIGATORIO";
                invalido = true;
                }
                else { Label8.Text = ""; }
                if (TxtEndereco.Text == string.Empty) { Label9.Text = "CAMPO OBRIGATORIO";
                invalido = true;
                }
                else { Label9.Text = ""; }
                if (TxtBairro.Text == string.Empty) { Label11.Text = "CAMPO OBRIGATORIO";
                invalido = true;
                }
                else { Label11.Text = ""; }
                if (TxtCEP.Text == string.Empty) { Label10.Text = "CAMPO OBRIGATORIO"; 
                    invalido = true; }
                else { Label10.Text = ""; }
                if (txtUsuario.Text == string.Empty)  { Label16.Text = "CAMPO OBRIGATORIO"; 
                    invalido = true;}
                else { Label16.Text = ""; }
                if (TxtSenha.Text == string.Empty) {    Label18.Text = "CAMPO OBRIGATORIO"; 
                    invalido = true;}
                else { Label18.Text = ""; }

                if (ExistenciaDeUsuario(txtUsuario.Text) == true)
                {
                    //Se nenhum campo estiver vazio, liberar o acesso
                    if (txtNome.Text != string.Empty &&
                       TxtEndereco.Text != string.Empty &&
                       TxtBairro.Text != string.Empty &&
                       TxtCEP.Text != string.Empty &&
                       TxtTelefone.Text != string.Empty &&
                       txtUsuario.Text != string.Empty &&
                       TxtSenha.Text != string.Empty) { invalido = false; }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
       }


        #endregion

        //Inserção do usuario no banco de dados após passar pela duas verificações acima
        #region CadastrarNovoUsuario
        private Boolean cadastrarNovoUsuario()
        {
            try
            {
              bd = new ProjectContext();
              CLIENTE newCliente = new CLIENTE();
              
              newCliente.NOME = txtNome.Text;
              newCliente.ENDERECO = TxtEndereco.Text;
              newCliente.CEP = TxtCEP.Text;
              newCliente.BAIRRO = TxtBairro.Text;
              newCliente.TELEFONE = TxtTelefone.Text;
              newCliente.UF = DropDownUf.SelectedItem.ToString();
              newCliente.CIDADE = DropDownCidade.SelectedItem.ToString();
              newCliente.USUARIO = txtUsuario.Text;
              newCliente.SENHA = TxtSenha.Text;

              bd.CLIENTEs.Add(newCliente);
              bd.SaveChanges();
              bd.Dispose();
              return true;
            }
            catch (Exception)
            {
                return false;
                throw;
               
            }
        }
        #endregion

        private void funcaoJS()
        {
            string[] aTextbox = { txtNome.ID, TxtEndereco.ID };
            //Caixa de Texto nome
            this.txtNome.Attributes.Add("onblur", "return validaCampos(txtNome);");
            this.txtNome.Attributes.Add("onkeydown", "return soLetra();");
            this.txtNome.Attributes.Add("onkeyup", "return AutoTabular(40, txtNome, TxtEndereco);");
            this.txtNome.Attributes.Add("onmouseup", "return noclick(txtNome);");
            this.txtNome.Attributes.Add("onclick", "return AlterarCampo(txtNome);");

            //Caixa de Texto Endereço
            this.TxtEndereco.Attributes.Add("onkeydown", "return soLetra();");
            this.TxtEndereco.Attributes.Add("onblur", "return validaCampos(TxtEndereco);");
            this.TxtEndereco.Attributes.Add("onkeyup", "return AutoTabular(60, TxtEndereco, TxtCEP);");
            this.TxtEndereco.Attributes.Add("onmouseup", "return noclick(TxtEndereco);");
            this.TxtEndereco.Attributes.Add("onclick", "return AlterarCampo(TxtEndereco);");


            //Caixa de Texto CEP
            this.TxtCEP.Attributes.Add("onkeypress", "return Mascara('CEP', TxtCEP);");
            this.TxtCEP.Attributes.Add("onblur", "return validaCampos(TxtCEP);");
            this.TxtCEP.Attributes.Add("onkeyup", "return AutoTabular(9, TxtCEP, TxtBairro);");
            this.TxtCEP.Attributes.Add("Onmouseup", "return noclick(TxtCEP);");
            this.TxtCEP.Attributes.Add("onclick", "return AlterarCampo(TxtCEP);");

            //Caixa de Texto Bairro
            this.TxtBairro.Attributes.Add("onkeydown", "return soLetra();");
            this.TxtBairro.Attributes.Add("onblur", "return validaCampos(TxtBairro);");
            this.TxtBairro.Attributes.Add("onkeyup", "return AutoTabular(30, TxtBairro, TxtUF);");
            this.TxtBairro.Attributes.Add("Onmouseup", "return noclick(TxtBairro);");
            this.TxtBairro.Attributes.Add("onclick", "return AlterarCampo(TxtBairro);");

            //Caixa de Texto Telefone
            this.TxtTelefone.Attributes.Add("onkeypress", "return Mascara('TELEFONE', TxtTelefone);");
            this.TxtTelefone.Attributes.Add("onkeydown", "return soNumero();");
            this.TxtTelefone.Attributes.Add("onkeyup", "return AutoTabular(9, TxtTelefone, txtUsuario);");
            this.TxtTelefone.Attributes.Add("Onmouseup", "return noclick(TxtTelefone);");

            //Caixa de Texto Usuario
            this.txtUsuario.Attributes.Add("onblur", "return validaCampos(txtUsuario);");
            this.txtUsuario.Attributes.Add("onkeydown", "return soLetra();");
            this.txtUsuario.Attributes.Add("onkeyup", "return AutoTabular(15, txtUsuario, TxtSenha);");
            this.txtUsuario.Attributes.Add("Onmouseup", "return noclick(txtUsuario);");

            //Caixa de Texto Senha
            //this.TxtSenha.Attributes.Add("onblur", "return validarSenha(TxtSenha);");
            //this.TxtSenha.Attributes.Add("Onmouseup", "return noclick(TxtSenha);");

            //this.btnEnviar.Attributes.Add("onclick", "return ValidarSenha(TxtSenha);");

            this.btnEnviar.Attributes.Add("onclick", "return testeCampos(aTextbox);");
        }


        protected void DropDownUf_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bd = new ProjectContext();
                string UF = DropDownUf.SelectedItem.ToString();

                var municipios = (from mun in bd.MUNICIPIOs where mun.UF == UF select mun.NOME).ToList();

                DropDownCidade.DataSource = municipios;
                DropDownCidade.DataBind();

            }
            catch (Exception)
            {
                
                throw;
            }

        }

    }
}