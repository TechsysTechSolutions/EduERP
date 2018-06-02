using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;
using System.Web.Security;
using System.Data.Entity;
using Techsys_School_ERP.Model.ViewModel;
using System.Web.Script.Serialization;

namespace Techsys_School_ERP.Controllers
{
	public class FeeController : Controller
	{
		int[] nFeeIdArr;
		public ActionResult TestHandsonDropdown()
		{
			return View();
		}

		public ActionResult TestSuminHandsonTable()
		{
			return View();
		}

		[HttpPost]
		public JsonResult getexam()
		{
			Dictionary<int, string> exam = new Dictionary<int, string>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				var query = dbcontext.Exam.Select(c => new
				{
					id = c.Id,
					name = c.Name
				}).ToList();
				var jsonSerialiser = new JavaScriptSerializer();
				var json = jsonSerialiser.Serialize(query);
				return Json(json, JsonRequestBehavior.AllowGet);

			}

		}
		#region FeeList
		// GET: Exam/FeeNameList
		[Authorize(Roles = "Admin")]
		public ActionResult FeeNameList()
		{
			List<FeeList_ViewModel> feeListViewModel = new List<FeeList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				feeListViewModel = (from usr in dbcontext.Users
									join fee in dbcontext.Fee on usr.Id equals fee.Created_By
									where fee.Is_Deleted == null || fee.Is_Deleted == false
									select new FeeList_ViewModel
									{
										Id = fee.Id,
										Name = fee.Name,
										Academic_Year = usr.Academic_Year,
										User_Id = usr.User_Id,
										Is_Deleted = usr.Is_Deleted,
										Created_On = fee.Created_On,
										Created_By = fee.Created_By

									}).ToList();


			}


			return View(feeListViewModel);
		}

		[HttpPost]
		public JsonResult CreateFee(string Fee_Name)
		{
			try
			{
				Fee newFee = new Fee();
				int nUser_Id;
				using (var dbcontext = new SchoolERPDBContext())
				{
					nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				}
				using (var dbcontext = new SchoolERPDBContext())
				{
					newFee.Name = Fee_Name;
					newFee.Academic_Year = (DateTime.Now.Month <= 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
					newFee.Is_Active = true;
					newFee.Created_By = nUser_Id;
					newFee.Created_On = DateTime.Now;
					if (dbcontext.Fee.Where(a => a.Name.Replace(" ", "").Trim().ToString() == Fee_Name.Replace(" ", "").Trim().ToString() && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Academic_Year == newFee.Academic_Year).Count() == 0)
					{
						dbcontext.Fee.Add(newFee);
						dbcontext.SaveChanges();
						return Json("OK", JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json("Fee Already Exists.", JsonRequestBehavior.AllowGet);
					}
				}

			}
			catch (Exception e)
			{
				return Json("Failure", JsonRequestBehavior.AllowGet);
			}


		}


		public async Task<ActionResult> Delete(int id)
		{
			using (var dbcontext = new SchoolERPDBContext())
			{


				Fee fee = await dbcontext.Fee.FindAsync(id);
				if (fee != null)
				{
					fee.Is_Deleted = true;
					dbcontext.Entry(fee).CurrentValues.SetValues(fee);
					dbcontext.Entry(fee).State = EntityState.Modified;
					await dbcontext.SaveChangesAsync();
				}
			}

			return RedirectToAction("FeeNameList");
		}
		#endregion

		#region FeeConfigure
		public ActionResult ConfigureFeesForClass()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var clsList = (from cls in dbcontext.Class select cls).ToList();
				if (clsList != null)
				{
					ViewBag.classList = clsList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
			return View();
		}

		[HttpPost]
		public JsonResult GetFeeConfiguration(string Class_Id , string Academic_Year)
		{
			List<FeeConfiguration_ViewModel> feeConfiguration_ViewModel = new List<Model.ViewModel.FeeConfiguration_ViewModel>();
			FeeConfiguration_ViewModel emptyFeeConfiguration_ViewModel = new FeeConfiguration_ViewModel();
			List< FeeConfiguration_ViewModel> editModefeeConfiguration_ViewModel = new List<Model.ViewModel.FeeConfiguration_ViewModel>();
			List<FeeConfiguration_ViewModel> newlyAddedfeeConfiguration_ViewModel = new List<Model.ViewModel.FeeConfiguration_ViewModel>();
			ViewData["Class_Id"] = Class_Id;
			long nAcademicYear = Convert.ToInt64(Academic_Year);
			//long nAcademicYear = (DateTime.Now.Month <= 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
			int nClass_Id = Convert.ToInt16(Class_Id);

			using (var dbcontext = new SchoolERPDBContext())
			{
				//If fees not configuration , we need retrieve fee component from "Fee" master table otherwise need to retrieve from "Fee_Configuration" table
				if (dbcontext.Fee_Configuration.Where(a => a.Class_Id == nClass_Id && a.Academic_Year == nAcademicYear && (a.Is_Deleted == false || a.Is_Deleted == null)).Count() == 0)
				{
					feeConfiguration_ViewModel = (from usr in dbcontext.Users
												  join fee in dbcontext.Fee on usr.Id equals fee.Created_By
												  where fee.Is_Deleted == null || fee.Is_Deleted == false
												  select new FeeConfiguration_ViewModel
												  {
													  Id = fee.Id,
													  Name = fee.Name,
													  Academic_Year = nAcademicYear,
													  User_Id = usr.User_Id,
													  Yearly_Amount = null,
													  First_Term_Amount = null,
													  Second_Term_Amount = null,
													  Third_Term_Amount = null

												  }).ToList();

					//emptyFeeConfiguration_ViewModel.Academic_Year = nAcademicYear;
					//emptyFeeConfiguration_ViewModel.Name = "TOTAL";
					feeConfiguration_ViewModel.Add(emptyFeeConfiguration_ViewModel);
					feeConfiguration_ViewModel.Add(emptyFeeConfiguration_ViewModel);

					nFeeIdArr = feeConfiguration_ViewModel.ToList().Select(l => l.Id).Distinct().ToArray();
					TempData["FeeIdArr"] = nFeeIdArr;
					TempData.Keep("FeeIdArr");
					TempData["Class_Id"] = Class_Id;
					TempData.Keep("Class_Id");
				}
				//If fee already configured ,we need to retrieve from "Fee_Configuartion" table 
				else
				{
					feeConfiguration_ViewModel = (from usr in dbcontext.Users
												  join fee in dbcontext.Fee on usr.Id equals fee.Created_By
												   join fc in dbcontext.Fee_Configuration on fee.Id equals fc.Fee_Id
												  where (fc.Is_Deleted == null || fee.Is_Deleted == false) && fc.Class_Id == nClass_Id && fc.Academic_Year == nAcademicYear
												  select new FeeConfiguration_ViewModel
												  {
													  Id = fee.Id,
													  Name = fee.Name,
													  Academic_Year = nAcademicYear,
													  User_Id = usr.User_Id,
													  Frequency = fc.Frequency,
													  Amount = fc.Amount
													 // Total = fc.Total
												  }).ToList();

					//If new fee is added in "Fee" table but fees structure is already configured for the section
					var newFeeComponent = (from fc in dbcontext.Fee_Configuration where fc.Class_Id == nClass_Id && fc.Academic_Year == nAcademicYear  select fc.Fee_Id).Distinct().ToList();


					List<int> lstOtherFeeComponent = (from e in dbcontext.Fee
								  select e.Id).Except(newFeeComponent).ToList();


					for (int i = 0; i < lstOtherFeeComponent.Count(); i++)
					{
						FeeConfiguration_ViewModel newlyAddedFeeComponent = new FeeConfiguration_ViewModel();
						newlyAddedFeeComponent.Id = lstOtherFeeComponent[i];
						int nTemp_ClassId = lstOtherFeeComponent[i];
						newlyAddedFeeComponent.Name = Convert.ToString(dbcontext.Fee.Where(x => x.Id == nTemp_ClassId).ToList()[0].Name);
						newlyAddedFeeComponent.Academic_Year = nAcademicYear;
						newlyAddedFeeComponent.First_Term_Amount = null;
						newlyAddedFeeComponent.Second_Term_Amount = null;
						newlyAddedFeeComponent.Third_Term_Amount = null;
						newlyAddedFeeComponent.Yearly_Amount = null;

						newlyAddedfeeConfiguration_ViewModel.Add(newlyAddedFeeComponent);
						
					}

					int[] nFeeIdArr = feeConfiguration_ViewModel.Select(l => l.Id).Distinct().ToArray();
					int[] nFreqArr = feeConfiguration_ViewModel.Select(l => l.Frequency).Distinct().ToArray();
					
					for (int nrecordLoopCount = 0; nrecordLoopCount < nFeeIdArr.Count(); nrecordLoopCount++)
					{
						FeeConfiguration_ViewModel oFeeConfiguration_ViewModel = new FeeConfiguration_ViewModel();
						oFeeConfiguration_ViewModel = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount]).ToList()[0];
						FeeConfiguration_ViewModel temp_oFeeConfiguration_ViewModel = new FeeConfiguration_ViewModel();
						temp_oFeeConfiguration_ViewModel.Id = oFeeConfiguration_ViewModel.Id;
						temp_oFeeConfiguration_ViewModel.Name = oFeeConfiguration_ViewModel.Name;
						temp_oFeeConfiguration_ViewModel.Academic_Year = oFeeConfiguration_ViewModel.Academic_Year;
					
						for (int nfreqLoopCount = 1; nfreqLoopCount <= nFreqArr.Count(); nfreqLoopCount++)
						{
							if (nfreqLoopCount == 1)
							{
								temp_oFeeConfiguration_ViewModel.Yearly_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 1).ToList()[0].Amount;
								//emptyFeeConfiguration_ViewModel.Yearly_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 1).ToList()[0].Total;

								continue;
							}
							if (nfreqLoopCount == 2)
							{
								temp_oFeeConfiguration_ViewModel.First_Term_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 2).ToList()[0].Amount;
								//emptyFeeConfiguration_ViewModel.First_Term_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 2).ToList()[0].Total;
								continue;
							}
							if (nfreqLoopCount == 3)
							{
								temp_oFeeConfiguration_ViewModel.Second_Term_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 3).ToList()[0].Amount;
							//	emptyFeeConfiguration_ViewModel.Second_Term_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 3).ToList()[0].Total;
								continue;
							}
							else
							{
								temp_oFeeConfiguration_ViewModel.Third_Term_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 4).ToList()[0].Amount;
								//emptyFeeConfiguration_ViewModel.Third_Term_Amount = feeConfiguration_ViewModel.Where(a => a.Id == nFeeIdArr[nrecordLoopCount] && a.Frequency == 4).ToList()[0].Total;
							}
						}
						editModefeeConfiguration_ViewModel.Add(temp_oFeeConfiguration_ViewModel);

					}

					

					feeConfiguration_ViewModel.Clear();
					feeConfiguration_ViewModel.AddRange(editModefeeConfiguration_ViewModel);
					//emptyFeeConfiguration_ViewModel.Academic_Year = nAcademicYear;
					//emptyFeeConfiguration_ViewModel.Name = "TOTAL";
					feeConfiguration_ViewModel.AddRange(newlyAddedfeeConfiguration_ViewModel);
					nFeeIdArr = feeConfiguration_ViewModel.ToList().Select(l => l.Id).Distinct().ToArray();
					TempData["FeeIdArr"] = nFeeIdArr;
					TempData.Keep("FeeIdArr");
					TempData["Class_Id"] = Class_Id;
					TempData.Keep("Class_Id");
					feeConfiguration_ViewModel.Add(emptyFeeConfiguration_ViewModel);
					feeConfiguration_ViewModel.Add(emptyFeeConfiguration_ViewModel);
					


				}


			}
			return Json(feeConfiguration_ViewModel.ToArray(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult SaveFeeConfiguration(List<string[]> myData , string Academic_Year )
		{
			Fee_Configuration newFeeConfig = new Fee_Configuration();
			int nUser_Id;
		
			nFeeIdArr = (int[])TempData.Peek("FeeIdArr");

			string sReturnText = string.Empty;
			using (var dbcontext = new SchoolERPDBContext())
			{
				//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				nUser_Id = 4;
			}
			int nCount = myData.ToList().Count;

			int nLoopCount = 0;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						for (int i = 0; i < nCount - 1; i++)
						{
							if (myData[i][0] != string.Empty && myData[i][0] != null)
							{
								for (int j = 0; j < 4; j++)
								{
									newFeeConfig.Class_Id = Convert.ToInt16(TempData.Peek("Class_Id"));
									newFeeConfig.Academic_Year = Convert.ToInt64(myData[i][1]);
									newFeeConfig.Fee_Id = Convert.ToInt16(nFeeIdArr[i]);
									newFeeConfig.Is_Active = true;
									if (j == 0)
									{
										newFeeConfig.Frequency = 1;
										newFeeConfig.Amount = (myData[i][2] == "") ? 0 : Convert.ToDecimal(myData[i][2]);
										newFeeConfig.Total = Convert.ToDecimal(myData[nCount - 1][2]);
									}
									else if (j == 1)
									{
										newFeeConfig.Frequency = 2;
										newFeeConfig.Amount = (myData[i][3] == "") ? 0 : Convert.ToDecimal(myData[i][3]);
										newFeeConfig.Total = Convert.ToDecimal(myData[nCount - 1][3]);
									}
									else if (j == 2)
									{
										newFeeConfig.Frequency = 3;
										newFeeConfig.Amount = (myData[i][4] == "") ? 0 : Convert.ToDecimal(myData[i][4]);
										newFeeConfig.Total = Convert.ToDecimal(myData[nCount - 1][4]);
									}
									else
									{
										newFeeConfig.Frequency = 4;
										newFeeConfig.Amount = (myData[i][5] == "") ? 0 : Convert.ToDecimal(myData[i][5]);
										newFeeConfig.Total = Convert.ToDecimal(myData[nCount - 1][5]);
									}
									newFeeConfig.Created_By = nUser_Id;
									newFeeConfig.Created_On = DateTime.Now;
									if (dbcontext.Fee_Configuration.Where(a => a.Class_Id == newFeeConfig.Class_Id && a.Fee_Id == newFeeConfig.Fee_Id && a.Academic_Year == newFeeConfig.Academic_Year && a.Frequency == newFeeConfig.Frequency && (a.Is_Deleted == false || a.Is_Deleted == null)).Count() == 0)
									{

										dbcontext.Fee_Configuration.Add(newFeeConfig);
										dbcontext.SaveChanges();
										if (nCount - 3 == i && j == 3)
										{
											transaction.Commit();
											sReturnText = "OK";
										}
									}
									else
									{
										
											var feeConfigId = dbcontext.Fee_Configuration.Where(x => x.Fee_Id == newFeeConfig.Fee_Id && x.Class_Id == newFeeConfig.Class_Id && x.Academic_Year == newFeeConfig.Academic_Year && x.Frequency == newFeeConfig.Frequency && (x.Is_Deleted == false || x.Is_Deleted == null)).FirstOrDefault().Id;

											Fee_Configuration feeConfigToBeUpdated = dbcontext.Fee_Configuration.Find(feeConfigId);

											feeConfigToBeUpdated.Fee_Id = newFeeConfig.Fee_Id;
											feeConfigToBeUpdated.Class_Id = newFeeConfig.Class_Id;
											feeConfigToBeUpdated.Frequency = newFeeConfig.Frequency;
											feeConfigToBeUpdated.Amount = newFeeConfig.Amount;
											feeConfigToBeUpdated.Academic_Year = newFeeConfig.Academic_Year;
											feeConfigToBeUpdated.Total = newFeeConfig.Total;
											feeConfigToBeUpdated.Is_Active = true;

											dbcontext.Entry(feeConfigToBeUpdated).State = EntityState.Modified;
											dbcontext.SaveChanges();
										
											if (nCount - 3 == i && j == 3)
											{
												transaction.Commit();
												sReturnText = "Updated";
											}
										
									}

								}

							}
							

						}
					}
					catch (Exception ex)
					{
						transaction.Rollback();
					}

				}
				return Json(sReturnText, JsonRequestBehavior.AllowGet);
			}
			
		}
		#endregion


	}
}
