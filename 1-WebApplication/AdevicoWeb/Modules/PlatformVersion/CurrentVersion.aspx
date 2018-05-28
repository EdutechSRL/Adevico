<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CurrentVersion.aspx.vb" Inherits="Comunita_OnLine.CurrentVersion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .Titolo_Campo
        {
            display:inline-block;
            min-width: 150px;
            vertical-align:top;
        }
        
        .Titolo_CampoSmall
        {
            display:inline-block;
            min-width: 85px;
            vertical-align:top;
        }


        .Testo_campo
        {
            display:inline-block;
            padding-right: 25px;
        }

        .Testo_campoSmall
        {
            display:inline-block;
            min-width: 75px;
            /*padding-right: 25px;*/
        }

        ul.main > li
        {
            padding: 1em;
            padding-bottom: 0.5em;
            padding-top: 0;

        }

        ul.main ul
        {
            display: inline-block;
            max-width: 600px;
        }
        ul.main ul li
        {
            list-style-type: circle;
        }

         ul.main ul li.update
        {
            list-style-type: disc;
            padding-bottom: 0.5em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Versioni piattaforma
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <h3>Versione corrente</h3>
    <span class="Titolo_Campo">Versione</span><span class="Testo_campo">e2540683</span>
    <br />
    <span class="Titolo_Campo">Data versione</span><span class="Testo_campo">24/05/2018</span>
    <br />
    <span class="Titolo_Campo">Base Url</span>
    <span class="Testo_campo">
        <asp:Literal ID="LITbaseUrl" runat="server"></asp:Literal>
    </span>
    <br />
    <h3>Dettagli versioni</h3>
    <ul class="month">
        <li>
            <h4>Maggio 2018</h4>
            <ul class="main">
                <li>
                    <span class="Titolo_CampoSmall">e2540683</span>
                    <span class="Titolo_CampoSmall">24/05/2018</span>
                    <ul>
                        <li class="update">
                           Update - Grafici questionari: a capo se lunghi, controllo contenuto testi.
                        </li>
                        <li class="fix">
                           fix - PF/Questionari: catch errore
                        </li>
                        <li class="update">
                           WIP - Import massivo: nuova pagina non linkata.
                        </li>
                    </ul>
                <li>
                    <span class="Titolo_CampoSmall">1ec06188</span>
                    <span class="Titolo_CampoSmall">15/05/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Cookie: tolto setting cookie che creavano problemi con l'autenticazione per WebAPI
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">1e545a50</span>
                    <span class="Titolo_CampoSmall">14/05/2018</span>
                    <ul>
                        <li class="update">
                           update - API Token: creati ed aggiornati sempre (login, logon as, cambio comunità, debugaccess, etc...)
                        </li>
                        <li class="update">
                           update - AuthRedirect.aspx: SE non ho l'utente, redirect su LOGIN e successivamente di nuovo alla pagina richiesta
                        </li>
                    </ul>

                </li>
            </ul>
            <h4>Aprile 2018</h4>
            <ul class="main">
                 <li>
                    <span class="Titolo_CampoSmall">4d8508e9</span>
                    <span class="Titolo_CampoSmall">30/04/2018</span>
                    <ul>
                        <li class="update">
                           update - questionari: esportazioni multicolonna ed allineamento csv, xml
                        </li>
                        <li class="fix">
                           fix - questionari: invio questionario
                        </li>
                        <li class="fix">
                           fix - visualizzazione difficoltà/codice domanda
                        </li>
                    </ul>

                </li>
                <li>
                    <span class="Titolo_CampoSmall">26b01066</span>
                    <span class="Titolo_CampoSmall">24/04/2018</span>
                    <ul>
                        <li class="update">
                           update - questionari: intestazioni esportazioni riga singola, testo avanti/indietro (testo compilazione esterna)
                        </li>
                        <li class="fix">
                           fix - questionari: arrotondamento totali, compilazione rating senza testo, 
                        </li>
                    </ul>

                </li>
                <li>
                    <span class="Titolo_CampoSmall">e2b57798</span>
                    <span class="Titolo_CampoSmall">20/04/2018</span>
                    <ul>
                        <li class="update">
                           update - questionari: grafica, testo avanti/indietro
                        </li>
                        <li class="fix">
                           fix - questionari: riordino
                        </li>
                        <li class="fix">
                           fix - questionari: tasto invia
                        </li>
                    </ul>

                </li>
                <li>
                    <span class="Titolo_CampoSmall">e2b57798</span>
                    <span class="Titolo_CampoSmall">11/04/2018</span>
                    <ul>
                        <li class="update">
                           update - questionari: revisioni grafiche compilazione, statistiche, anteprima
                        </li>
                        <li class="fix">
                           fix - questionari: tasto invia
                        </li>
                    </ul>

                </li>
                <li>
                    <span class="Titolo_CampoSmall">e2b57798</span>
                    <span class="Titolo_CampoSmall">11/04/2018</span>
                    <ul>
                        <li class="fix">
                           fix - bandi: media su riepilogo tabelle economiche ed esportazioni
                        </li>
                    </ul>

                </li>
                <li>
                    <span class="Titolo_CampoSmall">1661c58</span>
                    <span class="Titolo_CampoSmall">10/04/2018</span>
                    <ul>
                        <li class="update">
                           update - Aggiornamento statistiche questionari  (WIP)
                        </li>
                        <li class="update">
                           update - Miglioramento login
                        </li>
                        <li class="fix">
                           fix - Impostazioni commissioni avanzate bandi
                        </li>
                    </ul>

                </li>
            </ul>
            <h4>Marzo 2018</h4>
            <ul class="main">
                <li>
                    <span class="Titolo_CampoSmall">dd8657ce</span>
                    <span class="Titolo_CampoSmall">27/03/2018</span>
                    <ul>
                        <li class="update">
                           update - Aggiornamento statistiche questionari
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">d8d376c7</span>
                    <span class="Titolo_CampoSmall">02/03/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Tasto statistiche in play pacchetto
                        </li>
                    </ul>
                </li>
                 <li>
                    <span class="Titolo_CampoSmall">e3d77b9e</span>
                    <span class="Titolo_CampoSmall">01/03/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Creazione comunità: cancellazione cache comunità per il responsabile
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">d4664d51</span>
                    <span class="Titolo_CampoSmall">01/03/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Modifica comunità: nascosto testo federazione
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
        <li>
             <h4>Febbraio 2018</h4>

            <ul class="main">
                <li>
                    <span class="Titolo_CampoSmall">2745de9d</span>
                    <span class="Titolo_CampoSmall">21/02/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Percorso formativo e Questionari: blocco visualizzazione se non ci sono permessi a livello di percorso formativo
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">e098e47a</span>
                    <span class="Titolo_CampoSmall">20/02/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Questionari: blocco salvataggio
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">5b4a1cc5</span>
                    <span class="Titolo_CampoSmall">16/02/2018</span>
                    <ul>
                        <li class="fix">
                           fix - EduPath: possibilità di impostare per un partecipante se possa o meno vedere le proprie statistiche
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">5d8a0805</span>
                    <span class="Titolo_CampoSmall">16/02/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Questionari: evitare doppie sottomissioni da inviti in casi particolari 
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">86e5f83b</span>
                    <span class="Titolo_CampoSmall">14/02/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Stato bandi/adesioni in modifica sotto alcune condizioni
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">c79cbc0f</span>
                    <span class="Titolo_CampoSmall">12/02/2018</span>
                    <ul>
                        <li class="fix">
                           fix - sottomissioni questionari su invito
                        </li>
                    </ul>
                </li>
            </ul>
            <h4>Gennaio 2018</h4>

            <ul class="main">
                <li>
                    <span class="Titolo_CampoSmall">850632ee</span>
                    <span class="Titolo_CampoSmall">31/01/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Modifica impostazioni materiale scorm: formattazione radiobutton
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">03b8f410</span>
                    <span class="Titolo_CampoSmall">30/01/2018</span>
                    <ul>
                        <li class="fix">
                           fix - Materiale SCORM: primo salvataggio ignora le impostazioni
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">35e103c</span>
                    <span class="Titolo_CampoSmall">30/01/2018</span>
                    <ul>
                        <li class="update">
                           Update - Bandi - Aggiunta esportazione tabelle Economiche in Excel con formattazioni.
                        </li>
                    </ul>
                </li>
                <li><span class="Titolo_CampoSmall">fa6f5955</span>
                    <span class="Titolo_CampoSmall">25/01/2018</span>
                    <ul>
                        <li class="update">
                            Update - Possibilità di cancellare le statistiche degli utenti disiscritti della comunità.<br />
                            Tali utenti risultavano invisibili nelle statistiche, ma bloccavano la possibilità di modifica del percorso formativo.
                            E' inoltre possibile cancellera le statistiche di utenti non amministratori, togliendoli dalla comunità e bloccandone così l'accesso fino al completamento della cancellazione.
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">fa987fd7</span>
                    <span class="Titolo_CampoSmall">25/01/2018</span>
                    <ul>
                        <li class="fix">
                           fix - modifica responsabile comunità.
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">43e48e22</span>
                    <span class="Titolo_CampoSmall">23/01/2018</span>            
                    <ul>
                        <li class="fix">
                           fix - Nell'albero delle cartelle del repository rimanevano visibili le cartelle cestinate, finchè non venissero cancellate fisicamente.
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">f8d3898</span>
                    <span class="Titolo_CampoSmall">23/01/2018</span>
                    <ul>
                        <li class="update">
                           Update - Aggiornati i controlli telerik.<br />
                            Per maggiori informazioni, si rimanda al sito <a href="https://www.telerik.com/support/whats-new/aspnet-ajax/release-history/ui-for-asp-net-ajax-r1-2018-(version-2018-1-117)">Telerik</a>.
                        </li>
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">7ba42788</span>
                    <span class="Titolo_CampoSmall">18/01/2018</span>
                    <ul>
                        <li class="update">
                           Update - Nella gestione Skin è stata aggiunta la possibilità di verificare i dati caricati dato l'ID della skin.
                        </li>
                        <li class="fix">
                           fix - Nella gestione Skin, impostando un ID comunità non valido per i test, la piattaforma dava errore
                        </li>                        
                    </ul>
                </li>
                <li>
                    <span class="Titolo_CampoSmall">1d2af39</span>
                    <span class="Titolo_CampoSmall">15/01/2018</span>
                    <ul>
                        <li class="fix">
                           fix - certificati: l'esportazione di certificati legati ai percorsi formativi gestisce nuovamente i tag relativi ai Webinar (dove previsti) ed ai questionari.
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
</asp:Content>
