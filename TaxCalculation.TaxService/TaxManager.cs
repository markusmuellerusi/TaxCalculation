using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TaxCalculation.Common;
using TaxCalculation.Dtos;

namespace TaxCalculation.TaxService
{
    public class TaxManager : ITaxManager, ITaxCalculator
    {
        private TaxCodes _taxCodes;
        // ReSharper disable once NotAccessedField.Local
        private IDateTimeService _dateTimeService;
        private static Dictionary<string, Type> _calculators;

        public TaxManager(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
            _calculators = new Dictionary<string, Type>();
            Init();
        }

        private void Init()
        {
            //Read tax codes and tax rates from storage
            _taxCodes = new TaxCodes()
            {
                new TaxCode
                {
                    TaxCodeId = 1,
                    TaxCodeName = "Tax Free",
                    TaxRates = new TaxRates
                    {
                        new TaxRate
                        {
                            TaxRateId = 1,
                            TaxRateName = "Tax free",
                            Rate = 0m,
                            StartDate = DateTime.MinValue,
                            EndDate = DateTime.MaxValue
                        }
                    }
                },
                new TaxCode
                {
                    TaxCodeId = 2,
                    TaxCodeName = "Normal Tax",
                    TaxRates = new TaxRates
                    {
                        new TaxRate
                        {
                            TaxRateId = 1,
                            TaxRateName = "Normal Taxrate 10%",
                            Rate = .1m,
                            StartDate = DateTime.MinValue,
                            EndDate = DateTime.MaxValue
                        }
                    }
                },
                new TaxCode
                {
                    TaxCodeId = 3,
                    TaxCodeName = "Additional Tax for imported products",
                    TaxRates = new TaxRates
                    {
                        new TaxRate
                        {
                            TaxRateId = 1,
                            TaxRateName = "Additional Taxrate 5%",
                            Rate = .05m,
                            StartDate = DateTime.MinValue,
                            EndDate = DateTime.MaxValue
                        }
                    }//,
                    //Every tax code can be extended by special calculation rules
                    //TaxCalculator = "TaxCalculation.SpecialTaxRule.dll;TaxCalculation.SpecialTaxRule.Calculator"
                }
            };
        }

        public TaxCode GetTaxCode(int taxCodeId)
        {
            return _taxCodes.FirstOrDefault(c => c.TaxCodeId == taxCodeId);
        }

        public void CalcTax(TransactionDocument doc)
        {
            foreach (var lineItem in doc.LineItems.Where(lineItem => lineItem.LineItemType == LineItemType.Product))
            {
                if (lineItem.TaxCodes != null && lineItem.TaxCodes.Any())
                {
                    var tax = 0m;
                    foreach (var taxCode in lineItem.TaxCodes)
                    {
                        tax += CalTax(false, doc.DocDate,
                            taxCode.TaxCodeId, lineItem.Quantity, lineItem.NetPrice);
                    }

                    lineItem.TaxTotal = tax * lineItem.Quantity;
                    lineItem.GrossPrice = lineItem.NetPrice + tax;
                    lineItem.NetTotal = lineItem.NetPrice * lineItem.Quantity;
                    lineItem.GrossTotal = lineItem.NetTotal + lineItem.TaxTotal;
                }

                doc.NetTotal += lineItem.NetTotal;
                doc.GrossTotal += lineItem.GrossTotal;
            }
        }

        public decimal CalTax(bool includeQuantity, DateTime docDate, int taxCodeId, decimal quantity, decimal price)
        {
            var taxCode = GetTaxCode(taxCodeId);
            if (taxCode == null)
                throw new ArgumentOutOfRangeException(nameof(taxCodeId));

            var taxRate = taxCode.TaxRates.FirstOrDefault(r => r.StartDate < docDate && docDate < r.EndDate);
            if (taxRate == null)
                throw new ArgumentOutOfRangeException(nameof(taxRate));


            if (string.IsNullOrEmpty(taxCode.TaxCalculator))
                return CalcTax(includeQuantity, quantity, price, taxRate.Rate);

            //Use a special Tax Calculator if specified!
            var calculatorType = CreateType(taxCode.TaxCalculator);
            var calculator = CreateInstance<ITaxCalculator>(calculatorType);
            return calculator.CalcTax(includeQuantity, quantity, price, taxRate.Rate);
        }

        public decimal CalcTax(bool includeQuantity, decimal quantity, decimal price, decimal rate)
        {
            if (includeQuantity)
            {
                return Math.Round(price * rate * 20.0M + 0.499m, MidpointRounding.AwayFromZero) * 0.05M;
            }

            var result = Math.Round(price * rate * 20.0M + 0.499m, MidpointRounding.AwayFromZero) * 0.05M;
            return result;
        }


        private static T CreateInstance<T>(Type type) where T : class
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!(Activator.CreateInstance(type) is T result))
                throw new Exception($"Cannot create instance of type '{type.FullName}'");

            return result;
        }

        private static Type CreateType(string assemblyAndClass)
        {
            if (_calculators.ContainsKey(assemblyAndClass))
                return _calculators[assemblyAndClass];

            var assemblyAndClassParts = assemblyAndClass.Split(';');
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (path != null) assemblyAndClassParts[0] = Path.Combine(path, assemblyAndClassParts[0]);
            var type = Assembly.LoadFrom(assemblyAndClassParts[0]).GetType(assemblyAndClassParts[1]);
            if (!_calculators.ContainsKey(assemblyAndClass))
                _calculators.Add(assemblyAndClass, type);
            return type;
        }
    }
}
