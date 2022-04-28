using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Correios_Services.Exception;
using Correios_Services.Extensions;
using Correios_Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Correios_Services
{
    public class Parser
    {
       
        public static Package ParsePackage(string html)
        {
            try
            {
                var document = new HtmlParser().ParseDocument(html);
                var packageCode = ParsePackageCode(document);
                var package = new Package(packageCode);
                package.AddTrackingInfo(ParsePackageTracking(document));
                return package;
            }
            catch (ParseException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw new ParseException("Não foi possível converter o pacote/encomenda.", ex);
            }
        }

        private static string ParsePackageCode(IHtmlDocument document)
        {
            try
            {
                var code = document.QuerySelector("#page > main > .sub_header_in > .container > h1").Text().Replace("Rastreamento de Objeto - ", string.Empty);

                if (string.IsNullOrEmpty(code))
                    throw new ParseException("Código da encomenda/pacote não foi encontrado.");

                return code;
            }
            catch (ParseException ex)
            {
                throw ex;
            }
            catch (System.Exception)
            {
                throw new ParseException("Código da encomenda/pacote não foi encontrado.");
            }
        }

        private static IEnumerable<PackageTracking> ParsePackageTracking(IHtmlDocument document)
        {
            var tracking = new List<PackageTracking>();

            PackageTracking trackingStatus = null;
            var statusLines = document.QuerySelectorAll(".singlepost > ul.linha_status");
            if (statusLines.Length == 0)
                throw new ParseException("Postagem não encontrada e/ou Aguardando postagem pelo remetente.");

            const string packageDateTimePattern = @"[\w\s\:]*(\d{2}\/\d{2}\/\d{4})[\w\|\s\:]*(\d{2}\:\d{2})";

            try
            {
                foreach (var lines in statusLines.Select(ul => ul.Children))
                {
                    trackingStatus = new PackageTracking();
                    trackingStatus.Status = lines[0].QuerySelector("b").Text().RemoveLineEndings();
                    trackingStatus.Date = lines[1].Text().ExtractDateTime(packageDateTimePattern);
                    trackingStatus.Source = lines[2].Text().RemoveLineEndings().Replace("Origem: ", string.Empty).Replace("Local: ", string.Empty);

                    if (lines.Length >= 4)
                        trackingStatus.Destination = lines[3].Text().RemoveLineEndings().Replace("Destino: ", string.Empty);

                    tracking.Add(trackingStatus);
                }
            }
            catch (System.Exception ex)
            {
                throw new ParseException("Não foi possível converter o pacote/encomenda.", ex);
            }

            if (tracking.Count() == 0)
                throw new ParseException("Rastreamento não encontrado.");

            return tracking;
        }
    }
}
