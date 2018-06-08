using LaboratoryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LaboratoryWeb.Controllers
{
    public class LaboratoryController : Controller
    {
        [ValidateInput(false)]
        public ActionResult Index(LaboratoryViewModel viewModel)
        {
            viewModel.ErrorMessage = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                // Create a few sample experiments for demonstartion purposes
                if (viewModel.Samples == null || viewModel.Reagents == null && viewModel.Repititions == null)
                {
                    viewModel.Samples = serializer.Serialize(new string[][]
                    {
                    new string[] {Samples.Blood.ToString(), Samples.Plazma.ToString()},
                    new string[] {Samples.RedCell.ToString(), Samples.WhiteCell.ToString()},
                    new string[] {Samples.Blood.ToString()}
                    });

                    viewModel.Reagents = serializer.Serialize(new string[][]
                    {
                    new string[] {Reagents.Acid.ToString(), Reagents.Phosphate.ToString()},
                    new string[] {Reagents.H2O.ToString(), Reagents.Sulphate.ToString()},
                    new string[] {Reagents.Sulphate.ToString(), Reagents.H2O.ToString(), Reagents.Acid.ToString()}
                    });

                    viewModel.Repititions = serializer.Serialize(new int[] { 1, 3,5 });
                }

                var samples = serializer.Deserialize<string[][]>(viewModel.Samples);
                var reagents = serializer.Deserialize<string[][]>(viewModel.Reagents);
                var replicates = serializer.Deserialize<int[]>(viewModel.Repititions);


                viewModel.Plates = new Laboratory().RunExperiments((int)viewModel.PlateSize, samples, reagents, replicates);
                viewModel.Output = serializer.Serialize(viewModel.Plates);
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
            }

            return View(viewModel);
        }
    }
}