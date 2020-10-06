﻿using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace IcanNet
{
    public class IcanValidator : IIcanValidator
    {
        public IcanNetResult Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return IcanNetResult.ValueMissing; }

            if (value.Length < 2) { return IcanNetResult.ValueTooSmall; }

            var countryCode = value.Substring(0, 2).ToUpper();

            int lengthForCountryCode;

            var countryCodeKnown = _lengths.TryGetValue(countryCode, out lengthForCountryCode);
            if (!countryCodeKnown)
            {
                return IcanNetResult.CountryCodeNotKnown;
            }

            // Check length.
            if (value.Length < lengthForCountryCode) { return IcanNetResult.ValueTooSmall; }
            if (value.Length > lengthForCountryCode) { return IcanNetResult.ValueTooBig; }

            var upperValue = value.ToUpper();
            var newIcan = upperValue.Substring(4) + upperValue.Substring(0, 4);

            newIcan = Regex.Replace(newIcan, @"\D", match => (match.Value[0] - 55).ToString(CultureInfo.InvariantCulture));

            var remainder = BigInteger.Parse(newIcan) % 97;

            if (remainder != 1) { return IcanNetResult.ValueFailsModule97Check; }

            return IcanNetResult.IsValid;
        }

        private static readonly Dictionary<string, int> _lengths = new Dictionary<string, int>
        {
            { "AD", 24 },	//	Andorra
            { "AE", 23 },	//	United Arab Emirates
            { "AL", 28 },	//	Albania
            { "AO", 25 },	//	Angola
            { "AT", 20 },	//	Austria
            { "AZ", 28 },	//	Azerbaijan
            { "BA", 20 },	//	Bosnia-Hercegovina
            { "BE", 16 },	//	Belgium
            { "BF", 27 },	//	Burkina Faso
            { "BG", 22 },	//	Bulgaria
            { "BH", 22 },	//	Bahrain
            { "BI", 16 },	//	Burundi
            { "BJ", 28 },	//	Benin
            { "BR", 29 },	//	Brazil
            { "BY", 28 },	//	Belarus
            { "CH", 21 },	//	Switzerland
            { "CI", 28 },	//	Ivory Coast
            { "CM", 27 },	//	Cameroon
            { "CR", 22 },	//	Costa Rica
            { "CV", 25 },	//	Cape Verde
            { "CY", 28 },	//	Cyprus
            { "CZ", 24 },	//	Czech Republic
            { "DE", 22 },	//	Germany
            { "DK", 18 },	//	Denmark
            { "DO", 28 },	//	Dominican Republic
            { "DZ", 24 },	//	Algeria
            { "EE", 20 },	//	Estonia
            { "ES", 24 },	//	Spain
            { "FI", 18 },	//	Finland
            { "FO", 18 },	//	Faeroe Islands
            { "FR", 27 },	//	France
            { "GB", 22 },	//	Great Britain
            { "GE", 22 },	//	Georgia
            { "GI", 23 },	//	Gibraltar
            { "GL", 18 },	//	Greenland
            { "GR", 27 },	//	Greece
            { "GT", 28 },	//	Guatemala
            { "HR", 21 },	//	Croatia
            { "HU", 28 },	//	Hungary
            { "IE", 22 },	//	Ireland
            { "IL", 23 },	//	Israel
            { "IM", 22 },	//	Isle of Man
            { "IQ", 23 },	//	Iraq
            { "IR", 26 },	//	Iran
            { "IS", 26 },	//	Iceland
            { "IT", 27 },	//	Italy
            { "JE", 22 },	//	Jersey
            { "JO", 30 },	//	Jordan
            { "KW", 30 },	//	Kuwait
            { "KZ", 20 },	//	Kasakhstan
            { "LB", 28 },	//	Lebanon
            { "LC", 32 },	//	St Lucia
            { "LI", 21 },	//	Liechtenstein
            { "LT", 20 },	//	Lithuania
            { "LU", 20 },	//	Luxembourg
            { "LV", 21 },	//	Latvia
            { "MC", 27 },	//	Monaco
            { "MD", 24 },	//	Moldova
            { "ME", 22 },	//	Montenegro
            { "MG", 27 },	//	Madagascar
            { "MK", 19 },	//	Macedonia
            { "ML", 28 },	//	Mali
            { "MR", 27 },	//	Mauritania
            { "MT", 31 },	//	Malta
            { "MU", 30 },	//	Mauritius
            { "MZ", 25 },	//	Mozambique
            { "NL", 18 },	//	Netherlands
            { "NO", 15 },	//	Norway
            { "PK", 24 },	//	Pakistan
            { "PL", 28 },	//	Poland
            { "PS", 29 },	//	Palestinian Territory
            { "PT", 25 },	//	Portugal
            { "QA", 29 },	//	Qatar
            { "RO", 24 },	//	Romania
            { "RS", 22 },	//	Serbia
            { "SA", 24 },	//	Saudi Arabia
            { "SC", 31 },	//	Seychelles
            { "SE", 24 },	//	Sweden
            { "SI", 19 },	//	Slovenia
            { "SK", 24 },	//	Slovakia
            { "SM", 27 },	//	San Marino
            { "SN", 28 },	//	Senegal
            { "ST", 25 },	//	São Tomé og Príncipe
            { "SV", 28 },	//	El Salvador
            { "TL", 23 },	//	Timor-Leste
            { "TN", 24 },	//	Tunisia
            { "TR", 26 },	//	Turkey
            { "UA", 29 },	//	Ukraine
            { "VG", 24 },	//	Virgin Islands
            { "XK", 20 },   //	Kosovo
            //  Crypto
            { "CB", 44 },   //	Core Blockchain Livenet
            { "AB", 44 },   //	Core Blockchain Testnet
            { "CE", 44 }    //	Core Enterprise Blockchain Privatenet
        };
    }
}
