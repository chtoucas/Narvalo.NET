// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Currencies
{
    using System;

    using Xunit;
    
    public static class CurrencyFacts
    {
        #region Currency

        [Fact]
        public static void Currency_IsNotNull()
        {
            // Act & Assert
            Assert.True(ADP.Currency != null);
            Assert.True(AED.Currency != null);
            Assert.True(AFA.Currency != null);
            Assert.True(AFN.Currency != null);
            Assert.True(ALK.Currency != null);
            Assert.True(ALL.Currency != null);
            Assert.True(AMD.Currency != null);
            Assert.True(ANG.Currency != null);
            Assert.True(AOA.Currency != null);
            Assert.True(AOK.Currency != null);
            Assert.True(AON.Currency != null);
            Assert.True(AOR.Currency != null);
            Assert.True(ARA.Currency != null);
            Assert.True(ARP.Currency != null);
            Assert.True(ARS.Currency != null);
            Assert.True(ARY.Currency != null);
            Assert.True(ATS.Currency != null);
            Assert.True(AUD.Currency != null);
            Assert.True(AWG.Currency != null);
            Assert.True(AYM.Currency != null);
            Assert.True(AZM.Currency != null);
            Assert.True(AZN.Currency != null);
            Assert.True(BAD.Currency != null);
            Assert.True(BAM.Currency != null);
            Assert.True(BBD.Currency != null);
            Assert.True(BDT.Currency != null);
            Assert.True(BEC.Currency != null);
            Assert.True(BEF.Currency != null);
            Assert.True(BEL.Currency != null);
            Assert.True(BGJ.Currency != null);
            Assert.True(BGK.Currency != null);
            Assert.True(BGL.Currency != null);
            Assert.True(BGN.Currency != null);
            Assert.True(BHD.Currency != null);
            Assert.True(BIF.Currency != null);
            Assert.True(BMD.Currency != null);
            Assert.True(BND.Currency != null);
            Assert.True(BOB.Currency != null);
            Assert.True(BOP.Currency != null);
            Assert.True(BOV.Currency != null);
            Assert.True(BRB.Currency != null);
            Assert.True(BRC.Currency != null);
            Assert.True(BRE.Currency != null);
            Assert.True(BRL.Currency != null);
            Assert.True(BRN.Currency != null);
            Assert.True(BRR.Currency != null);
            Assert.True(BSD.Currency != null);
            Assert.True(BTN.Currency != null);
            Assert.True(BUK.Currency != null);
            Assert.True(BWP.Currency != null);
            Assert.True(BYB.Currency != null);
            Assert.True(BYR.Currency != null);
            Assert.True(BZD.Currency != null);
            Assert.True(CAD.Currency != null);
            Assert.True(CDF.Currency != null);
            Assert.True(CHC.Currency != null);
            Assert.True(CHE.Currency != null);
            Assert.True(CHF.Currency != null);
            Assert.True(CHW.Currency != null);
            Assert.True(CLF.Currency != null);
            Assert.True(CLP.Currency != null);
            Assert.True(CNX.Currency != null);
            Assert.True(CNY.Currency != null);
            Assert.True(COP.Currency != null);
            Assert.True(COU.Currency != null);
            Assert.True(CRC.Currency != null);
            Assert.True(CSD.Currency != null);
            Assert.True(CSJ.Currency != null);
            Assert.True(CSK.Currency != null);
            Assert.True(CUC.Currency != null);
            Assert.True(CUP.Currency != null);
            Assert.True(CVE.Currency != null);
            Assert.True(CYP.Currency != null);
            Assert.True(CZK.Currency != null);
            Assert.True(DDM.Currency != null);
            Assert.True(DEM.Currency != null);
            Assert.True(DJF.Currency != null);
            Assert.True(DKK.Currency != null);
            Assert.True(DOP.Currency != null);
            Assert.True(DZD.Currency != null);
            Assert.True(ECS.Currency != null);
            Assert.True(ECV.Currency != null);
            Assert.True(EEK.Currency != null);
            Assert.True(EGP.Currency != null);
            Assert.True(EQE.Currency != null);
            Assert.True(ERN.Currency != null);
            Assert.True(ESA.Currency != null);
            Assert.True(ESB.Currency != null);
            Assert.True(ESP.Currency != null);
            Assert.True(ETB.Currency != null);
            Assert.True(EUR.Currency != null);
            Assert.True(FIM.Currency != null);
            Assert.True(FJD.Currency != null);
            Assert.True(FKP.Currency != null);
            Assert.True(FRF.Currency != null);
            Assert.True(GBP.Currency != null);
            Assert.True(GEK.Currency != null);
            Assert.True(GEL.Currency != null);
            Assert.True(GHC.Currency != null);
            Assert.True(GHP.Currency != null);
            Assert.True(GHS.Currency != null);
            Assert.True(GIP.Currency != null);
            Assert.True(GMD.Currency != null);
            Assert.True(GNE.Currency != null);
            Assert.True(GNF.Currency != null);
            Assert.True(GNS.Currency != null);
            Assert.True(GQE.Currency != null);
            Assert.True(GRD.Currency != null);
            Assert.True(GTQ.Currency != null);
            Assert.True(GWE.Currency != null);
            Assert.True(GWP.Currency != null);
            Assert.True(GYD.Currency != null);
            Assert.True(HKD.Currency != null);
            Assert.True(HNL.Currency != null);
            Assert.True(HRD.Currency != null);
            Assert.True(HRK.Currency != null);
            Assert.True(HTG.Currency != null);
            Assert.True(HUF.Currency != null);
            Assert.True(IDR.Currency != null);
            Assert.True(IEP.Currency != null);
            Assert.True(ILP.Currency != null);
            Assert.True(ILR.Currency != null);
            Assert.True(ILS.Currency != null);
            Assert.True(INR.Currency != null);
            Assert.True(IQD.Currency != null);
            Assert.True(IRR.Currency != null);
            Assert.True(ISJ.Currency != null);
            Assert.True(ISK.Currency != null);
            Assert.True(ITL.Currency != null);
            Assert.True(JMD.Currency != null);
            Assert.True(JOD.Currency != null);
            Assert.True(JPY.Currency != null);
            Assert.True(KES.Currency != null);
            Assert.True(KGS.Currency != null);
            Assert.True(KHR.Currency != null);
            Assert.True(KMF.Currency != null);
            Assert.True(KPW.Currency != null);
            Assert.True(KRW.Currency != null);
            Assert.True(KWD.Currency != null);
            Assert.True(KYD.Currency != null);
            Assert.True(KZT.Currency != null);
            Assert.True(LAJ.Currency != null);
            Assert.True(LAK.Currency != null);
            Assert.True(LBP.Currency != null);
            Assert.True(LKR.Currency != null);
            Assert.True(LRD.Currency != null);
            Assert.True(LSL.Currency != null);
            Assert.True(LSM.Currency != null);
            Assert.True(LTL.Currency != null);
            Assert.True(LTT.Currency != null);
            Assert.True(LUC.Currency != null);
            Assert.True(LUF.Currency != null);
            Assert.True(LUL.Currency != null);
            Assert.True(LVL.Currency != null);
            Assert.True(LVR.Currency != null);
            Assert.True(LYD.Currency != null);
            Assert.True(MAD.Currency != null);
            Assert.True(MAF.Currency != null);
            Assert.True(MDL.Currency != null);
            Assert.True(MGA.Currency != null);
            Assert.True(MGF.Currency != null);
            Assert.True(MKD.Currency != null);
            Assert.True(MLF.Currency != null);
            Assert.True(MMK.Currency != null);
            Assert.True(MNT.Currency != null);
            Assert.True(MOP.Currency != null);
            Assert.True(MRO.Currency != null);
            Assert.True(MTL.Currency != null);
            Assert.True(MTP.Currency != null);
            Assert.True(MUR.Currency != null);
            Assert.True(MVQ.Currency != null);
            Assert.True(MVR.Currency != null);
            Assert.True(MWK.Currency != null);
            Assert.True(MXN.Currency != null);
            Assert.True(MXP.Currency != null);
            Assert.True(MXV.Currency != null);
            Assert.True(MYR.Currency != null);
            Assert.True(MZE.Currency != null);
            Assert.True(MZM.Currency != null);
            Assert.True(MZN.Currency != null);
            Assert.True(NAD.Currency != null);
            Assert.True(NGN.Currency != null);
            Assert.True(NIC.Currency != null);
            Assert.True(NIO.Currency != null);
            Assert.True(NLG.Currency != null);
            Assert.True(NOK.Currency != null);
            Assert.True(NPR.Currency != null);
            Assert.True(NZD.Currency != null);
            Assert.True(OMR.Currency != null);
            Assert.True(PAB.Currency != null);
            Assert.True(PEH.Currency != null);
            Assert.True(PEI.Currency != null);
            Assert.True(PEN.Currency != null);
            Assert.True(PES.Currency != null);
            Assert.True(PGK.Currency != null);
            Assert.True(PHP.Currency != null);
            Assert.True(PKR.Currency != null);
            Assert.True(PLN.Currency != null);
            Assert.True(PLZ.Currency != null);
            Assert.True(PTE.Currency != null);
            Assert.True(PYG.Currency != null);
            Assert.True(QAR.Currency != null);
            Assert.True(RHD.Currency != null);
            Assert.True(ROK.Currency != null);
            Assert.True(ROL.Currency != null);
            Assert.True(RON.Currency != null);
            Assert.True(RSD.Currency != null);
            Assert.True(RUB.Currency != null);
            Assert.True(RUR.Currency != null);
            Assert.True(RWF.Currency != null);
            Assert.True(SAR.Currency != null);
            Assert.True(SBD.Currency != null);
            Assert.True(SCR.Currency != null);
            Assert.True(SDD.Currency != null);
            Assert.True(SDG.Currency != null);
            Assert.True(SDP.Currency != null);
            Assert.True(SEK.Currency != null);
            Assert.True(SGD.Currency != null);
            Assert.True(SHP.Currency != null);
            Assert.True(SIT.Currency != null);
            Assert.True(SKK.Currency != null);
            Assert.True(SLL.Currency != null);
            Assert.True(SOS.Currency != null);
            Assert.True(SRD.Currency != null);
            Assert.True(SRG.Currency != null);
            Assert.True(SSP.Currency != null);
            Assert.True(STD.Currency != null);
            Assert.True(SUR.Currency != null);
            Assert.True(SVC.Currency != null);
            Assert.True(SYP.Currency != null);
            Assert.True(SZL.Currency != null);
            Assert.True(THB.Currency != null);
            Assert.True(TJR.Currency != null);
            Assert.True(TJS.Currency != null);
            Assert.True(TMM.Currency != null);
            Assert.True(TMT.Currency != null);
            Assert.True(TND.Currency != null);
            Assert.True(TOP.Currency != null);
            Assert.True(TPE.Currency != null);
            Assert.True(TRL.Currency != null);
            Assert.True(TRY.Currency != null);
            Assert.True(TTD.Currency != null);
            Assert.True(TWD.Currency != null);
            Assert.True(TZS.Currency != null);
            Assert.True(UAH.Currency != null);
            Assert.True(UAK.Currency != null);
            Assert.True(UGS.Currency != null);
            Assert.True(UGW.Currency != null);
            Assert.True(UGX.Currency != null);
            Assert.True(USD.Currency != null);
            Assert.True(USN.Currency != null);
            Assert.True(USS.Currency != null);
            Assert.True(UYI.Currency != null);
            Assert.True(UYN.Currency != null);
            Assert.True(UYP.Currency != null);
            Assert.True(UYU.Currency != null);
            Assert.True(UZS.Currency != null);
            Assert.True(VEB.Currency != null);
            Assert.True(VEF.Currency != null);
            Assert.True(VNC.Currency != null);
            Assert.True(VND.Currency != null);
            Assert.True(VUV.Currency != null);
            Assert.True(WST.Currency != null);
            Assert.True(XAF.Currency != null);
            Assert.True(XAG.Currency != null);
            Assert.True(XAU.Currency != null);
            Assert.True(XBA.Currency != null);
            Assert.True(XBB.Currency != null);
            Assert.True(XBC.Currency != null);
            Assert.True(XBD.Currency != null);
            Assert.True(XCD.Currency != null);
            Assert.True(XDR.Currency != null);
            Assert.True(XEU.Currency != null);
            Assert.True(XFO.Currency != null);
            Assert.True(XFU.Currency != null);
            Assert.True(XOF.Currency != null);
            Assert.True(XPD.Currency != null);
            Assert.True(XPF.Currency != null);
            Assert.True(XPT.Currency != null);
            Assert.True(XRE.Currency != null);
            Assert.True(XSU.Currency != null);
            Assert.True(XTS.Currency != null);
            Assert.True(XUA.Currency != null);
            Assert.True(XXX.Currency != null);
            Assert.True(YDD.Currency != null);
            Assert.True(YER.Currency != null);
            Assert.True(YUD.Currency != null);
            Assert.True(YUM.Currency != null);
            Assert.True(YUN.Currency != null);
            Assert.True(ZAL.Currency != null);
            Assert.True(ZAR.Currency != null);
            Assert.True(ZMK.Currency != null);
            Assert.True(ZMW.Currency != null);
            Assert.True(ZRN.Currency != null);
            Assert.True(ZRZ.Currency != null);
            Assert.True(ZWC.Currency != null);
            Assert.True(ZWD.Currency != null);
            Assert.True(ZWL.Currency != null);
            Assert.True(ZWN.Currency != null);
            Assert.True(ZWR.Currency != null);
        }

        #endregion
        
        [Fact]
        public static void Currencies_SatisfyStructuralEqualityRules()
        {
            // Act & Assert
            Assert.True(ADP.Currency.Equals(Currency.Of("ADP")));
            Assert.True(ADP.Currency == Currency.Of("ADP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ADP"), Currency.Of("ADP")));
            Assert.False(Object.ReferenceEquals(ADP.Currency, Currency.Of("ADP")));

            Assert.True(AED.Currency.Equals(Currency.Of("AED")));
            Assert.True(AED.Currency == Currency.Of("AED"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AED"), Currency.Of("AED")));
            Assert.False(Object.ReferenceEquals(AED.Currency, Currency.Of("AED")));

            Assert.True(AFA.Currency.Equals(Currency.Of("AFA")));
            Assert.True(AFA.Currency == Currency.Of("AFA"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AFA"), Currency.Of("AFA")));
            Assert.False(Object.ReferenceEquals(AFA.Currency, Currency.Of("AFA")));

            Assert.True(AFN.Currency.Equals(Currency.Of("AFN")));
            Assert.True(AFN.Currency == Currency.Of("AFN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AFN"), Currency.Of("AFN")));
            Assert.False(Object.ReferenceEquals(AFN.Currency, Currency.Of("AFN")));

            Assert.True(ALK.Currency.Equals(Currency.Of("ALK")));
            Assert.True(ALK.Currency == Currency.Of("ALK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ALK"), Currency.Of("ALK")));
            Assert.False(Object.ReferenceEquals(ALK.Currency, Currency.Of("ALK")));

            Assert.True(ALL.Currency.Equals(Currency.Of("ALL")));
            Assert.True(ALL.Currency == Currency.Of("ALL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ALL"), Currency.Of("ALL")));
            Assert.False(Object.ReferenceEquals(ALL.Currency, Currency.Of("ALL")));

            Assert.True(AMD.Currency.Equals(Currency.Of("AMD")));
            Assert.True(AMD.Currency == Currency.Of("AMD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AMD"), Currency.Of("AMD")));
            Assert.False(Object.ReferenceEquals(AMD.Currency, Currency.Of("AMD")));

            Assert.True(ANG.Currency.Equals(Currency.Of("ANG")));
            Assert.True(ANG.Currency == Currency.Of("ANG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ANG"), Currency.Of("ANG")));
            Assert.False(Object.ReferenceEquals(ANG.Currency, Currency.Of("ANG")));

            Assert.True(AOA.Currency.Equals(Currency.Of("AOA")));
            Assert.True(AOA.Currency == Currency.Of("AOA"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AOA"), Currency.Of("AOA")));
            Assert.False(Object.ReferenceEquals(AOA.Currency, Currency.Of("AOA")));

            Assert.True(AOK.Currency.Equals(Currency.Of("AOK")));
            Assert.True(AOK.Currency == Currency.Of("AOK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AOK"), Currency.Of("AOK")));
            Assert.False(Object.ReferenceEquals(AOK.Currency, Currency.Of("AOK")));

            Assert.True(AON.Currency.Equals(Currency.Of("AON")));
            Assert.True(AON.Currency == Currency.Of("AON"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AON"), Currency.Of("AON")));
            Assert.False(Object.ReferenceEquals(AON.Currency, Currency.Of("AON")));

            Assert.True(AOR.Currency.Equals(Currency.Of("AOR")));
            Assert.True(AOR.Currency == Currency.Of("AOR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AOR"), Currency.Of("AOR")));
            Assert.False(Object.ReferenceEquals(AOR.Currency, Currency.Of("AOR")));

            Assert.True(ARA.Currency.Equals(Currency.Of("ARA")));
            Assert.True(ARA.Currency == Currency.Of("ARA"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ARA"), Currency.Of("ARA")));
            Assert.False(Object.ReferenceEquals(ARA.Currency, Currency.Of("ARA")));

            Assert.True(ARP.Currency.Equals(Currency.Of("ARP")));
            Assert.True(ARP.Currency == Currency.Of("ARP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ARP"), Currency.Of("ARP")));
            Assert.False(Object.ReferenceEquals(ARP.Currency, Currency.Of("ARP")));

            Assert.True(ARS.Currency.Equals(Currency.Of("ARS")));
            Assert.True(ARS.Currency == Currency.Of("ARS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ARS"), Currency.Of("ARS")));
            Assert.False(Object.ReferenceEquals(ARS.Currency, Currency.Of("ARS")));

            Assert.True(ARY.Currency.Equals(Currency.Of("ARY")));
            Assert.True(ARY.Currency == Currency.Of("ARY"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ARY"), Currency.Of("ARY")));
            Assert.False(Object.ReferenceEquals(ARY.Currency, Currency.Of("ARY")));

            Assert.True(ATS.Currency.Equals(Currency.Of("ATS")));
            Assert.True(ATS.Currency == Currency.Of("ATS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ATS"), Currency.Of("ATS")));
            Assert.False(Object.ReferenceEquals(ATS.Currency, Currency.Of("ATS")));

            Assert.True(AUD.Currency.Equals(Currency.Of("AUD")));
            Assert.True(AUD.Currency == Currency.Of("AUD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AUD"), Currency.Of("AUD")));
            Assert.False(Object.ReferenceEquals(AUD.Currency, Currency.Of("AUD")));

            Assert.True(AWG.Currency.Equals(Currency.Of("AWG")));
            Assert.True(AWG.Currency == Currency.Of("AWG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AWG"), Currency.Of("AWG")));
            Assert.False(Object.ReferenceEquals(AWG.Currency, Currency.Of("AWG")));

            Assert.True(AYM.Currency.Equals(Currency.Of("AYM")));
            Assert.True(AYM.Currency == Currency.Of("AYM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AYM"), Currency.Of("AYM")));
            Assert.False(Object.ReferenceEquals(AYM.Currency, Currency.Of("AYM")));

            Assert.True(AZM.Currency.Equals(Currency.Of("AZM")));
            Assert.True(AZM.Currency == Currency.Of("AZM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AZM"), Currency.Of("AZM")));
            Assert.False(Object.ReferenceEquals(AZM.Currency, Currency.Of("AZM")));

            Assert.True(AZN.Currency.Equals(Currency.Of("AZN")));
            Assert.True(AZN.Currency == Currency.Of("AZN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("AZN"), Currency.Of("AZN")));
            Assert.False(Object.ReferenceEquals(AZN.Currency, Currency.Of("AZN")));

            Assert.True(BAD.Currency.Equals(Currency.Of("BAD")));
            Assert.True(BAD.Currency == Currency.Of("BAD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BAD"), Currency.Of("BAD")));
            Assert.False(Object.ReferenceEquals(BAD.Currency, Currency.Of("BAD")));

            Assert.True(BAM.Currency.Equals(Currency.Of("BAM")));
            Assert.True(BAM.Currency == Currency.Of("BAM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BAM"), Currency.Of("BAM")));
            Assert.False(Object.ReferenceEquals(BAM.Currency, Currency.Of("BAM")));

            Assert.True(BBD.Currency.Equals(Currency.Of("BBD")));
            Assert.True(BBD.Currency == Currency.Of("BBD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BBD"), Currency.Of("BBD")));
            Assert.False(Object.ReferenceEquals(BBD.Currency, Currency.Of("BBD")));

            Assert.True(BDT.Currency.Equals(Currency.Of("BDT")));
            Assert.True(BDT.Currency == Currency.Of("BDT"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BDT"), Currency.Of("BDT")));
            Assert.False(Object.ReferenceEquals(BDT.Currency, Currency.Of("BDT")));

            Assert.True(BEC.Currency.Equals(Currency.Of("BEC")));
            Assert.True(BEC.Currency == Currency.Of("BEC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BEC"), Currency.Of("BEC")));
            Assert.False(Object.ReferenceEquals(BEC.Currency, Currency.Of("BEC")));

            Assert.True(BEF.Currency.Equals(Currency.Of("BEF")));
            Assert.True(BEF.Currency == Currency.Of("BEF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BEF"), Currency.Of("BEF")));
            Assert.False(Object.ReferenceEquals(BEF.Currency, Currency.Of("BEF")));

            Assert.True(BEL.Currency.Equals(Currency.Of("BEL")));
            Assert.True(BEL.Currency == Currency.Of("BEL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BEL"), Currency.Of("BEL")));
            Assert.False(Object.ReferenceEquals(BEL.Currency, Currency.Of("BEL")));

            Assert.True(BGJ.Currency.Equals(Currency.Of("BGJ")));
            Assert.True(BGJ.Currency == Currency.Of("BGJ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BGJ"), Currency.Of("BGJ")));
            Assert.False(Object.ReferenceEquals(BGJ.Currency, Currency.Of("BGJ")));

            Assert.True(BGK.Currency.Equals(Currency.Of("BGK")));
            Assert.True(BGK.Currency == Currency.Of("BGK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BGK"), Currency.Of("BGK")));
            Assert.False(Object.ReferenceEquals(BGK.Currency, Currency.Of("BGK")));

            Assert.True(BGL.Currency.Equals(Currency.Of("BGL")));
            Assert.True(BGL.Currency == Currency.Of("BGL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BGL"), Currency.Of("BGL")));
            Assert.False(Object.ReferenceEquals(BGL.Currency, Currency.Of("BGL")));

            Assert.True(BGN.Currency.Equals(Currency.Of("BGN")));
            Assert.True(BGN.Currency == Currency.Of("BGN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BGN"), Currency.Of("BGN")));
            Assert.False(Object.ReferenceEquals(BGN.Currency, Currency.Of("BGN")));

            Assert.True(BHD.Currency.Equals(Currency.Of("BHD")));
            Assert.True(BHD.Currency == Currency.Of("BHD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BHD"), Currency.Of("BHD")));
            Assert.False(Object.ReferenceEquals(BHD.Currency, Currency.Of("BHD")));

            Assert.True(BIF.Currency.Equals(Currency.Of("BIF")));
            Assert.True(BIF.Currency == Currency.Of("BIF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BIF"), Currency.Of("BIF")));
            Assert.False(Object.ReferenceEquals(BIF.Currency, Currency.Of("BIF")));

            Assert.True(BMD.Currency.Equals(Currency.Of("BMD")));
            Assert.True(BMD.Currency == Currency.Of("BMD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BMD"), Currency.Of("BMD")));
            Assert.False(Object.ReferenceEquals(BMD.Currency, Currency.Of("BMD")));

            Assert.True(BND.Currency.Equals(Currency.Of("BND")));
            Assert.True(BND.Currency == Currency.Of("BND"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BND"), Currency.Of("BND")));
            Assert.False(Object.ReferenceEquals(BND.Currency, Currency.Of("BND")));

            Assert.True(BOB.Currency.Equals(Currency.Of("BOB")));
            Assert.True(BOB.Currency == Currency.Of("BOB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BOB"), Currency.Of("BOB")));
            Assert.False(Object.ReferenceEquals(BOB.Currency, Currency.Of("BOB")));

            Assert.True(BOP.Currency.Equals(Currency.Of("BOP")));
            Assert.True(BOP.Currency == Currency.Of("BOP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BOP"), Currency.Of("BOP")));
            Assert.False(Object.ReferenceEquals(BOP.Currency, Currency.Of("BOP")));

            Assert.True(BOV.Currency.Equals(Currency.Of("BOV")));
            Assert.True(BOV.Currency == Currency.Of("BOV"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BOV"), Currency.Of("BOV")));
            Assert.False(Object.ReferenceEquals(BOV.Currency, Currency.Of("BOV")));

            Assert.True(BRB.Currency.Equals(Currency.Of("BRB")));
            Assert.True(BRB.Currency == Currency.Of("BRB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BRB"), Currency.Of("BRB")));
            Assert.False(Object.ReferenceEquals(BRB.Currency, Currency.Of("BRB")));

            Assert.True(BRC.Currency.Equals(Currency.Of("BRC")));
            Assert.True(BRC.Currency == Currency.Of("BRC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BRC"), Currency.Of("BRC")));
            Assert.False(Object.ReferenceEquals(BRC.Currency, Currency.Of("BRC")));

            Assert.True(BRE.Currency.Equals(Currency.Of("BRE")));
            Assert.True(BRE.Currency == Currency.Of("BRE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BRE"), Currency.Of("BRE")));
            Assert.False(Object.ReferenceEquals(BRE.Currency, Currency.Of("BRE")));

            Assert.True(BRL.Currency.Equals(Currency.Of("BRL")));
            Assert.True(BRL.Currency == Currency.Of("BRL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BRL"), Currency.Of("BRL")));
            Assert.False(Object.ReferenceEquals(BRL.Currency, Currency.Of("BRL")));

            Assert.True(BRN.Currency.Equals(Currency.Of("BRN")));
            Assert.True(BRN.Currency == Currency.Of("BRN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BRN"), Currency.Of("BRN")));
            Assert.False(Object.ReferenceEquals(BRN.Currency, Currency.Of("BRN")));

            Assert.True(BRR.Currency.Equals(Currency.Of("BRR")));
            Assert.True(BRR.Currency == Currency.Of("BRR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BRR"), Currency.Of("BRR")));
            Assert.False(Object.ReferenceEquals(BRR.Currency, Currency.Of("BRR")));

            Assert.True(BSD.Currency.Equals(Currency.Of("BSD")));
            Assert.True(BSD.Currency == Currency.Of("BSD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BSD"), Currency.Of("BSD")));
            Assert.False(Object.ReferenceEquals(BSD.Currency, Currency.Of("BSD")));

            Assert.True(BTN.Currency.Equals(Currency.Of("BTN")));
            Assert.True(BTN.Currency == Currency.Of("BTN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BTN"), Currency.Of("BTN")));
            Assert.False(Object.ReferenceEquals(BTN.Currency, Currency.Of("BTN")));

            Assert.True(BUK.Currency.Equals(Currency.Of("BUK")));
            Assert.True(BUK.Currency == Currency.Of("BUK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BUK"), Currency.Of("BUK")));
            Assert.False(Object.ReferenceEquals(BUK.Currency, Currency.Of("BUK")));

            Assert.True(BWP.Currency.Equals(Currency.Of("BWP")));
            Assert.True(BWP.Currency == Currency.Of("BWP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BWP"), Currency.Of("BWP")));
            Assert.False(Object.ReferenceEquals(BWP.Currency, Currency.Of("BWP")));

            Assert.True(BYB.Currency.Equals(Currency.Of("BYB")));
            Assert.True(BYB.Currency == Currency.Of("BYB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BYB"), Currency.Of("BYB")));
            Assert.False(Object.ReferenceEquals(BYB.Currency, Currency.Of("BYB")));

            Assert.True(BYR.Currency.Equals(Currency.Of("BYR")));
            Assert.True(BYR.Currency == Currency.Of("BYR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BYR"), Currency.Of("BYR")));
            Assert.False(Object.ReferenceEquals(BYR.Currency, Currency.Of("BYR")));

            Assert.True(BZD.Currency.Equals(Currency.Of("BZD")));
            Assert.True(BZD.Currency == Currency.Of("BZD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("BZD"), Currency.Of("BZD")));
            Assert.False(Object.ReferenceEquals(BZD.Currency, Currency.Of("BZD")));

            Assert.True(CAD.Currency.Equals(Currency.Of("CAD")));
            Assert.True(CAD.Currency == Currency.Of("CAD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CAD"), Currency.Of("CAD")));
            Assert.False(Object.ReferenceEquals(CAD.Currency, Currency.Of("CAD")));

            Assert.True(CDF.Currency.Equals(Currency.Of("CDF")));
            Assert.True(CDF.Currency == Currency.Of("CDF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CDF"), Currency.Of("CDF")));
            Assert.False(Object.ReferenceEquals(CDF.Currency, Currency.Of("CDF")));

            Assert.True(CHC.Currency.Equals(Currency.Of("CHC")));
            Assert.True(CHC.Currency == Currency.Of("CHC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CHC"), Currency.Of("CHC")));
            Assert.False(Object.ReferenceEquals(CHC.Currency, Currency.Of("CHC")));

            Assert.True(CHE.Currency.Equals(Currency.Of("CHE")));
            Assert.True(CHE.Currency == Currency.Of("CHE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CHE"), Currency.Of("CHE")));
            Assert.False(Object.ReferenceEquals(CHE.Currency, Currency.Of("CHE")));

            Assert.True(CHF.Currency.Equals(Currency.Of("CHF")));
            Assert.True(CHF.Currency == Currency.Of("CHF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CHF"), Currency.Of("CHF")));
            Assert.False(Object.ReferenceEquals(CHF.Currency, Currency.Of("CHF")));

            Assert.True(CHW.Currency.Equals(Currency.Of("CHW")));
            Assert.True(CHW.Currency == Currency.Of("CHW"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CHW"), Currency.Of("CHW")));
            Assert.False(Object.ReferenceEquals(CHW.Currency, Currency.Of("CHW")));

            Assert.True(CLF.Currency.Equals(Currency.Of("CLF")));
            Assert.True(CLF.Currency == Currency.Of("CLF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CLF"), Currency.Of("CLF")));
            Assert.False(Object.ReferenceEquals(CLF.Currency, Currency.Of("CLF")));

            Assert.True(CLP.Currency.Equals(Currency.Of("CLP")));
            Assert.True(CLP.Currency == Currency.Of("CLP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CLP"), Currency.Of("CLP")));
            Assert.False(Object.ReferenceEquals(CLP.Currency, Currency.Of("CLP")));

            Assert.True(CNX.Currency.Equals(Currency.Of("CNX")));
            Assert.True(CNX.Currency == Currency.Of("CNX"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CNX"), Currency.Of("CNX")));
            Assert.False(Object.ReferenceEquals(CNX.Currency, Currency.Of("CNX")));

            Assert.True(CNY.Currency.Equals(Currency.Of("CNY")));
            Assert.True(CNY.Currency == Currency.Of("CNY"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CNY"), Currency.Of("CNY")));
            Assert.False(Object.ReferenceEquals(CNY.Currency, Currency.Of("CNY")));

            Assert.True(COP.Currency.Equals(Currency.Of("COP")));
            Assert.True(COP.Currency == Currency.Of("COP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("COP"), Currency.Of("COP")));
            Assert.False(Object.ReferenceEquals(COP.Currency, Currency.Of("COP")));

            Assert.True(COU.Currency.Equals(Currency.Of("COU")));
            Assert.True(COU.Currency == Currency.Of("COU"));
            Assert.True(Object.ReferenceEquals(Currency.Of("COU"), Currency.Of("COU")));
            Assert.False(Object.ReferenceEquals(COU.Currency, Currency.Of("COU")));

            Assert.True(CRC.Currency.Equals(Currency.Of("CRC")));
            Assert.True(CRC.Currency == Currency.Of("CRC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CRC"), Currency.Of("CRC")));
            Assert.False(Object.ReferenceEquals(CRC.Currency, Currency.Of("CRC")));

            Assert.True(CSD.Currency.Equals(Currency.Of("CSD")));
            Assert.True(CSD.Currency == Currency.Of("CSD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CSD"), Currency.Of("CSD")));
            Assert.False(Object.ReferenceEquals(CSD.Currency, Currency.Of("CSD")));

            Assert.True(CSJ.Currency.Equals(Currency.Of("CSJ")));
            Assert.True(CSJ.Currency == Currency.Of("CSJ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CSJ"), Currency.Of("CSJ")));
            Assert.False(Object.ReferenceEquals(CSJ.Currency, Currency.Of("CSJ")));

            Assert.True(CSK.Currency.Equals(Currency.Of("CSK")));
            Assert.True(CSK.Currency == Currency.Of("CSK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CSK"), Currency.Of("CSK")));
            Assert.False(Object.ReferenceEquals(CSK.Currency, Currency.Of("CSK")));

            Assert.True(CUC.Currency.Equals(Currency.Of("CUC")));
            Assert.True(CUC.Currency == Currency.Of("CUC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CUC"), Currency.Of("CUC")));
            Assert.False(Object.ReferenceEquals(CUC.Currency, Currency.Of("CUC")));

            Assert.True(CUP.Currency.Equals(Currency.Of("CUP")));
            Assert.True(CUP.Currency == Currency.Of("CUP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CUP"), Currency.Of("CUP")));
            Assert.False(Object.ReferenceEquals(CUP.Currency, Currency.Of("CUP")));

            Assert.True(CVE.Currency.Equals(Currency.Of("CVE")));
            Assert.True(CVE.Currency == Currency.Of("CVE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CVE"), Currency.Of("CVE")));
            Assert.False(Object.ReferenceEquals(CVE.Currency, Currency.Of("CVE")));

            Assert.True(CYP.Currency.Equals(Currency.Of("CYP")));
            Assert.True(CYP.Currency == Currency.Of("CYP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CYP"), Currency.Of("CYP")));
            Assert.False(Object.ReferenceEquals(CYP.Currency, Currency.Of("CYP")));

            Assert.True(CZK.Currency.Equals(Currency.Of("CZK")));
            Assert.True(CZK.Currency == Currency.Of("CZK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("CZK"), Currency.Of("CZK")));
            Assert.False(Object.ReferenceEquals(CZK.Currency, Currency.Of("CZK")));

            Assert.True(DDM.Currency.Equals(Currency.Of("DDM")));
            Assert.True(DDM.Currency == Currency.Of("DDM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("DDM"), Currency.Of("DDM")));
            Assert.False(Object.ReferenceEquals(DDM.Currency, Currency.Of("DDM")));

            Assert.True(DEM.Currency.Equals(Currency.Of("DEM")));
            Assert.True(DEM.Currency == Currency.Of("DEM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("DEM"), Currency.Of("DEM")));
            Assert.False(Object.ReferenceEquals(DEM.Currency, Currency.Of("DEM")));

            Assert.True(DJF.Currency.Equals(Currency.Of("DJF")));
            Assert.True(DJF.Currency == Currency.Of("DJF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("DJF"), Currency.Of("DJF")));
            Assert.False(Object.ReferenceEquals(DJF.Currency, Currency.Of("DJF")));

            Assert.True(DKK.Currency.Equals(Currency.Of("DKK")));
            Assert.True(DKK.Currency == Currency.Of("DKK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("DKK"), Currency.Of("DKK")));
            Assert.False(Object.ReferenceEquals(DKK.Currency, Currency.Of("DKK")));

            Assert.True(DOP.Currency.Equals(Currency.Of("DOP")));
            Assert.True(DOP.Currency == Currency.Of("DOP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("DOP"), Currency.Of("DOP")));
            Assert.False(Object.ReferenceEquals(DOP.Currency, Currency.Of("DOP")));

            Assert.True(DZD.Currency.Equals(Currency.Of("DZD")));
            Assert.True(DZD.Currency == Currency.Of("DZD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("DZD"), Currency.Of("DZD")));
            Assert.False(Object.ReferenceEquals(DZD.Currency, Currency.Of("DZD")));

            Assert.True(ECS.Currency.Equals(Currency.Of("ECS")));
            Assert.True(ECS.Currency == Currency.Of("ECS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ECS"), Currency.Of("ECS")));
            Assert.False(Object.ReferenceEquals(ECS.Currency, Currency.Of("ECS")));

            Assert.True(ECV.Currency.Equals(Currency.Of("ECV")));
            Assert.True(ECV.Currency == Currency.Of("ECV"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ECV"), Currency.Of("ECV")));
            Assert.False(Object.ReferenceEquals(ECV.Currency, Currency.Of("ECV")));

            Assert.True(EEK.Currency.Equals(Currency.Of("EEK")));
            Assert.True(EEK.Currency == Currency.Of("EEK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("EEK"), Currency.Of("EEK")));
            Assert.False(Object.ReferenceEquals(EEK.Currency, Currency.Of("EEK")));

            Assert.True(EGP.Currency.Equals(Currency.Of("EGP")));
            Assert.True(EGP.Currency == Currency.Of("EGP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("EGP"), Currency.Of("EGP")));
            Assert.False(Object.ReferenceEquals(EGP.Currency, Currency.Of("EGP")));

            Assert.True(EQE.Currency.Equals(Currency.Of("EQE")));
            Assert.True(EQE.Currency == Currency.Of("EQE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("EQE"), Currency.Of("EQE")));
            Assert.False(Object.ReferenceEquals(EQE.Currency, Currency.Of("EQE")));

            Assert.True(ERN.Currency.Equals(Currency.Of("ERN")));
            Assert.True(ERN.Currency == Currency.Of("ERN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ERN"), Currency.Of("ERN")));
            Assert.False(Object.ReferenceEquals(ERN.Currency, Currency.Of("ERN")));

            Assert.True(ESA.Currency.Equals(Currency.Of("ESA")));
            Assert.True(ESA.Currency == Currency.Of("ESA"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ESA"), Currency.Of("ESA")));
            Assert.False(Object.ReferenceEquals(ESA.Currency, Currency.Of("ESA")));

            Assert.True(ESB.Currency.Equals(Currency.Of("ESB")));
            Assert.True(ESB.Currency == Currency.Of("ESB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ESB"), Currency.Of("ESB")));
            Assert.False(Object.ReferenceEquals(ESB.Currency, Currency.Of("ESB")));

            Assert.True(ESP.Currency.Equals(Currency.Of("ESP")));
            Assert.True(ESP.Currency == Currency.Of("ESP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ESP"), Currency.Of("ESP")));
            Assert.False(Object.ReferenceEquals(ESP.Currency, Currency.Of("ESP")));

            Assert.True(ETB.Currency.Equals(Currency.Of("ETB")));
            Assert.True(ETB.Currency == Currency.Of("ETB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ETB"), Currency.Of("ETB")));
            Assert.False(Object.ReferenceEquals(ETB.Currency, Currency.Of("ETB")));

            Assert.True(EUR.Currency.Equals(Currency.Of("EUR")));
            Assert.True(EUR.Currency == Currency.Of("EUR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("EUR"), Currency.Of("EUR")));
            Assert.False(Object.ReferenceEquals(EUR.Currency, Currency.Of("EUR")));

            Assert.True(FIM.Currency.Equals(Currency.Of("FIM")));
            Assert.True(FIM.Currency == Currency.Of("FIM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("FIM"), Currency.Of("FIM")));
            Assert.False(Object.ReferenceEquals(FIM.Currency, Currency.Of("FIM")));

            Assert.True(FJD.Currency.Equals(Currency.Of("FJD")));
            Assert.True(FJD.Currency == Currency.Of("FJD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("FJD"), Currency.Of("FJD")));
            Assert.False(Object.ReferenceEquals(FJD.Currency, Currency.Of("FJD")));

            Assert.True(FKP.Currency.Equals(Currency.Of("FKP")));
            Assert.True(FKP.Currency == Currency.Of("FKP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("FKP"), Currency.Of("FKP")));
            Assert.False(Object.ReferenceEquals(FKP.Currency, Currency.Of("FKP")));

            Assert.True(FRF.Currency.Equals(Currency.Of("FRF")));
            Assert.True(FRF.Currency == Currency.Of("FRF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("FRF"), Currency.Of("FRF")));
            Assert.False(Object.ReferenceEquals(FRF.Currency, Currency.Of("FRF")));

            Assert.True(GBP.Currency.Equals(Currency.Of("GBP")));
            Assert.True(GBP.Currency == Currency.Of("GBP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GBP"), Currency.Of("GBP")));
            Assert.False(Object.ReferenceEquals(GBP.Currency, Currency.Of("GBP")));

            Assert.True(GEK.Currency.Equals(Currency.Of("GEK")));
            Assert.True(GEK.Currency == Currency.Of("GEK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GEK"), Currency.Of("GEK")));
            Assert.False(Object.ReferenceEquals(GEK.Currency, Currency.Of("GEK")));

            Assert.True(GEL.Currency.Equals(Currency.Of("GEL")));
            Assert.True(GEL.Currency == Currency.Of("GEL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GEL"), Currency.Of("GEL")));
            Assert.False(Object.ReferenceEquals(GEL.Currency, Currency.Of("GEL")));

            Assert.True(GHC.Currency.Equals(Currency.Of("GHC")));
            Assert.True(GHC.Currency == Currency.Of("GHC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GHC"), Currency.Of("GHC")));
            Assert.False(Object.ReferenceEquals(GHC.Currency, Currency.Of("GHC")));

            Assert.True(GHP.Currency.Equals(Currency.Of("GHP")));
            Assert.True(GHP.Currency == Currency.Of("GHP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GHP"), Currency.Of("GHP")));
            Assert.False(Object.ReferenceEquals(GHP.Currency, Currency.Of("GHP")));

            Assert.True(GHS.Currency.Equals(Currency.Of("GHS")));
            Assert.True(GHS.Currency == Currency.Of("GHS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GHS"), Currency.Of("GHS")));
            Assert.False(Object.ReferenceEquals(GHS.Currency, Currency.Of("GHS")));

            Assert.True(GIP.Currency.Equals(Currency.Of("GIP")));
            Assert.True(GIP.Currency == Currency.Of("GIP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GIP"), Currency.Of("GIP")));
            Assert.False(Object.ReferenceEquals(GIP.Currency, Currency.Of("GIP")));

            Assert.True(GMD.Currency.Equals(Currency.Of("GMD")));
            Assert.True(GMD.Currency == Currency.Of("GMD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GMD"), Currency.Of("GMD")));
            Assert.False(Object.ReferenceEquals(GMD.Currency, Currency.Of("GMD")));

            Assert.True(GNE.Currency.Equals(Currency.Of("GNE")));
            Assert.True(GNE.Currency == Currency.Of("GNE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GNE"), Currency.Of("GNE")));
            Assert.False(Object.ReferenceEquals(GNE.Currency, Currency.Of("GNE")));

            Assert.True(GNF.Currency.Equals(Currency.Of("GNF")));
            Assert.True(GNF.Currency == Currency.Of("GNF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GNF"), Currency.Of("GNF")));
            Assert.False(Object.ReferenceEquals(GNF.Currency, Currency.Of("GNF")));

            Assert.True(GNS.Currency.Equals(Currency.Of("GNS")));
            Assert.True(GNS.Currency == Currency.Of("GNS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GNS"), Currency.Of("GNS")));
            Assert.False(Object.ReferenceEquals(GNS.Currency, Currency.Of("GNS")));

            Assert.True(GQE.Currency.Equals(Currency.Of("GQE")));
            Assert.True(GQE.Currency == Currency.Of("GQE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GQE"), Currency.Of("GQE")));
            Assert.False(Object.ReferenceEquals(GQE.Currency, Currency.Of("GQE")));

            Assert.True(GRD.Currency.Equals(Currency.Of("GRD")));
            Assert.True(GRD.Currency == Currency.Of("GRD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GRD"), Currency.Of("GRD")));
            Assert.False(Object.ReferenceEquals(GRD.Currency, Currency.Of("GRD")));

            Assert.True(GTQ.Currency.Equals(Currency.Of("GTQ")));
            Assert.True(GTQ.Currency == Currency.Of("GTQ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GTQ"), Currency.Of("GTQ")));
            Assert.False(Object.ReferenceEquals(GTQ.Currency, Currency.Of("GTQ")));

            Assert.True(GWE.Currency.Equals(Currency.Of("GWE")));
            Assert.True(GWE.Currency == Currency.Of("GWE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GWE"), Currency.Of("GWE")));
            Assert.False(Object.ReferenceEquals(GWE.Currency, Currency.Of("GWE")));

            Assert.True(GWP.Currency.Equals(Currency.Of("GWP")));
            Assert.True(GWP.Currency == Currency.Of("GWP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GWP"), Currency.Of("GWP")));
            Assert.False(Object.ReferenceEquals(GWP.Currency, Currency.Of("GWP")));

            Assert.True(GYD.Currency.Equals(Currency.Of("GYD")));
            Assert.True(GYD.Currency == Currency.Of("GYD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("GYD"), Currency.Of("GYD")));
            Assert.False(Object.ReferenceEquals(GYD.Currency, Currency.Of("GYD")));

            Assert.True(HKD.Currency.Equals(Currency.Of("HKD")));
            Assert.True(HKD.Currency == Currency.Of("HKD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("HKD"), Currency.Of("HKD")));
            Assert.False(Object.ReferenceEquals(HKD.Currency, Currency.Of("HKD")));

            Assert.True(HNL.Currency.Equals(Currency.Of("HNL")));
            Assert.True(HNL.Currency == Currency.Of("HNL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("HNL"), Currency.Of("HNL")));
            Assert.False(Object.ReferenceEquals(HNL.Currency, Currency.Of("HNL")));

            Assert.True(HRD.Currency.Equals(Currency.Of("HRD")));
            Assert.True(HRD.Currency == Currency.Of("HRD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("HRD"), Currency.Of("HRD")));
            Assert.False(Object.ReferenceEquals(HRD.Currency, Currency.Of("HRD")));

            Assert.True(HRK.Currency.Equals(Currency.Of("HRK")));
            Assert.True(HRK.Currency == Currency.Of("HRK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("HRK"), Currency.Of("HRK")));
            Assert.False(Object.ReferenceEquals(HRK.Currency, Currency.Of("HRK")));

            Assert.True(HTG.Currency.Equals(Currency.Of("HTG")));
            Assert.True(HTG.Currency == Currency.Of("HTG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("HTG"), Currency.Of("HTG")));
            Assert.False(Object.ReferenceEquals(HTG.Currency, Currency.Of("HTG")));

            Assert.True(HUF.Currency.Equals(Currency.Of("HUF")));
            Assert.True(HUF.Currency == Currency.Of("HUF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("HUF"), Currency.Of("HUF")));
            Assert.False(Object.ReferenceEquals(HUF.Currency, Currency.Of("HUF")));

            Assert.True(IDR.Currency.Equals(Currency.Of("IDR")));
            Assert.True(IDR.Currency == Currency.Of("IDR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("IDR"), Currency.Of("IDR")));
            Assert.False(Object.ReferenceEquals(IDR.Currency, Currency.Of("IDR")));

            Assert.True(IEP.Currency.Equals(Currency.Of("IEP")));
            Assert.True(IEP.Currency == Currency.Of("IEP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("IEP"), Currency.Of("IEP")));
            Assert.False(Object.ReferenceEquals(IEP.Currency, Currency.Of("IEP")));

            Assert.True(ILP.Currency.Equals(Currency.Of("ILP")));
            Assert.True(ILP.Currency == Currency.Of("ILP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ILP"), Currency.Of("ILP")));
            Assert.False(Object.ReferenceEquals(ILP.Currency, Currency.Of("ILP")));

            Assert.True(ILR.Currency.Equals(Currency.Of("ILR")));
            Assert.True(ILR.Currency == Currency.Of("ILR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ILR"), Currency.Of("ILR")));
            Assert.False(Object.ReferenceEquals(ILR.Currency, Currency.Of("ILR")));

            Assert.True(ILS.Currency.Equals(Currency.Of("ILS")));
            Assert.True(ILS.Currency == Currency.Of("ILS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ILS"), Currency.Of("ILS")));
            Assert.False(Object.ReferenceEquals(ILS.Currency, Currency.Of("ILS")));

            Assert.True(INR.Currency.Equals(Currency.Of("INR")));
            Assert.True(INR.Currency == Currency.Of("INR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("INR"), Currency.Of("INR")));
            Assert.False(Object.ReferenceEquals(INR.Currency, Currency.Of("INR")));

            Assert.True(IQD.Currency.Equals(Currency.Of("IQD")));
            Assert.True(IQD.Currency == Currency.Of("IQD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("IQD"), Currency.Of("IQD")));
            Assert.False(Object.ReferenceEquals(IQD.Currency, Currency.Of("IQD")));

            Assert.True(IRR.Currency.Equals(Currency.Of("IRR")));
            Assert.True(IRR.Currency == Currency.Of("IRR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("IRR"), Currency.Of("IRR")));
            Assert.False(Object.ReferenceEquals(IRR.Currency, Currency.Of("IRR")));

            Assert.True(ISJ.Currency.Equals(Currency.Of("ISJ")));
            Assert.True(ISJ.Currency == Currency.Of("ISJ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ISJ"), Currency.Of("ISJ")));
            Assert.False(Object.ReferenceEquals(ISJ.Currency, Currency.Of("ISJ")));

            Assert.True(ISK.Currency.Equals(Currency.Of("ISK")));
            Assert.True(ISK.Currency == Currency.Of("ISK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ISK"), Currency.Of("ISK")));
            Assert.False(Object.ReferenceEquals(ISK.Currency, Currency.Of("ISK")));

            Assert.True(ITL.Currency.Equals(Currency.Of("ITL")));
            Assert.True(ITL.Currency == Currency.Of("ITL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ITL"), Currency.Of("ITL")));
            Assert.False(Object.ReferenceEquals(ITL.Currency, Currency.Of("ITL")));

            Assert.True(JMD.Currency.Equals(Currency.Of("JMD")));
            Assert.True(JMD.Currency == Currency.Of("JMD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("JMD"), Currency.Of("JMD")));
            Assert.False(Object.ReferenceEquals(JMD.Currency, Currency.Of("JMD")));

            Assert.True(JOD.Currency.Equals(Currency.Of("JOD")));
            Assert.True(JOD.Currency == Currency.Of("JOD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("JOD"), Currency.Of("JOD")));
            Assert.False(Object.ReferenceEquals(JOD.Currency, Currency.Of("JOD")));

            Assert.True(JPY.Currency.Equals(Currency.Of("JPY")));
            Assert.True(JPY.Currency == Currency.Of("JPY"));
            Assert.True(Object.ReferenceEquals(Currency.Of("JPY"), Currency.Of("JPY")));
            Assert.False(Object.ReferenceEquals(JPY.Currency, Currency.Of("JPY")));

            Assert.True(KES.Currency.Equals(Currency.Of("KES")));
            Assert.True(KES.Currency == Currency.Of("KES"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KES"), Currency.Of("KES")));
            Assert.False(Object.ReferenceEquals(KES.Currency, Currency.Of("KES")));

            Assert.True(KGS.Currency.Equals(Currency.Of("KGS")));
            Assert.True(KGS.Currency == Currency.Of("KGS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KGS"), Currency.Of("KGS")));
            Assert.False(Object.ReferenceEquals(KGS.Currency, Currency.Of("KGS")));

            Assert.True(KHR.Currency.Equals(Currency.Of("KHR")));
            Assert.True(KHR.Currency == Currency.Of("KHR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KHR"), Currency.Of("KHR")));
            Assert.False(Object.ReferenceEquals(KHR.Currency, Currency.Of("KHR")));

            Assert.True(KMF.Currency.Equals(Currency.Of("KMF")));
            Assert.True(KMF.Currency == Currency.Of("KMF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KMF"), Currency.Of("KMF")));
            Assert.False(Object.ReferenceEquals(KMF.Currency, Currency.Of("KMF")));

            Assert.True(KPW.Currency.Equals(Currency.Of("KPW")));
            Assert.True(KPW.Currency == Currency.Of("KPW"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KPW"), Currency.Of("KPW")));
            Assert.False(Object.ReferenceEquals(KPW.Currency, Currency.Of("KPW")));

            Assert.True(KRW.Currency.Equals(Currency.Of("KRW")));
            Assert.True(KRW.Currency == Currency.Of("KRW"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KRW"), Currency.Of("KRW")));
            Assert.False(Object.ReferenceEquals(KRW.Currency, Currency.Of("KRW")));

            Assert.True(KWD.Currency.Equals(Currency.Of("KWD")));
            Assert.True(KWD.Currency == Currency.Of("KWD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KWD"), Currency.Of("KWD")));
            Assert.False(Object.ReferenceEquals(KWD.Currency, Currency.Of("KWD")));

            Assert.True(KYD.Currency.Equals(Currency.Of("KYD")));
            Assert.True(KYD.Currency == Currency.Of("KYD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KYD"), Currency.Of("KYD")));
            Assert.False(Object.ReferenceEquals(KYD.Currency, Currency.Of("KYD")));

            Assert.True(KZT.Currency.Equals(Currency.Of("KZT")));
            Assert.True(KZT.Currency == Currency.Of("KZT"));
            Assert.True(Object.ReferenceEquals(Currency.Of("KZT"), Currency.Of("KZT")));
            Assert.False(Object.ReferenceEquals(KZT.Currency, Currency.Of("KZT")));

            Assert.True(LAJ.Currency.Equals(Currency.Of("LAJ")));
            Assert.True(LAJ.Currency == Currency.Of("LAJ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LAJ"), Currency.Of("LAJ")));
            Assert.False(Object.ReferenceEquals(LAJ.Currency, Currency.Of("LAJ")));

            Assert.True(LAK.Currency.Equals(Currency.Of("LAK")));
            Assert.True(LAK.Currency == Currency.Of("LAK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LAK"), Currency.Of("LAK")));
            Assert.False(Object.ReferenceEquals(LAK.Currency, Currency.Of("LAK")));

            Assert.True(LBP.Currency.Equals(Currency.Of("LBP")));
            Assert.True(LBP.Currency == Currency.Of("LBP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LBP"), Currency.Of("LBP")));
            Assert.False(Object.ReferenceEquals(LBP.Currency, Currency.Of("LBP")));

            Assert.True(LKR.Currency.Equals(Currency.Of("LKR")));
            Assert.True(LKR.Currency == Currency.Of("LKR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LKR"), Currency.Of("LKR")));
            Assert.False(Object.ReferenceEquals(LKR.Currency, Currency.Of("LKR")));

            Assert.True(LRD.Currency.Equals(Currency.Of("LRD")));
            Assert.True(LRD.Currency == Currency.Of("LRD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LRD"), Currency.Of("LRD")));
            Assert.False(Object.ReferenceEquals(LRD.Currency, Currency.Of("LRD")));

            Assert.True(LSL.Currency.Equals(Currency.Of("LSL")));
            Assert.True(LSL.Currency == Currency.Of("LSL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LSL"), Currency.Of("LSL")));
            Assert.False(Object.ReferenceEquals(LSL.Currency, Currency.Of("LSL")));

            Assert.True(LSM.Currency.Equals(Currency.Of("LSM")));
            Assert.True(LSM.Currency == Currency.Of("LSM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LSM"), Currency.Of("LSM")));
            Assert.False(Object.ReferenceEquals(LSM.Currency, Currency.Of("LSM")));

            Assert.True(LTL.Currency.Equals(Currency.Of("LTL")));
            Assert.True(LTL.Currency == Currency.Of("LTL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LTL"), Currency.Of("LTL")));
            Assert.False(Object.ReferenceEquals(LTL.Currency, Currency.Of("LTL")));

            Assert.True(LTT.Currency.Equals(Currency.Of("LTT")));
            Assert.True(LTT.Currency == Currency.Of("LTT"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LTT"), Currency.Of("LTT")));
            Assert.False(Object.ReferenceEquals(LTT.Currency, Currency.Of("LTT")));

            Assert.True(LUC.Currency.Equals(Currency.Of("LUC")));
            Assert.True(LUC.Currency == Currency.Of("LUC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LUC"), Currency.Of("LUC")));
            Assert.False(Object.ReferenceEquals(LUC.Currency, Currency.Of("LUC")));

            Assert.True(LUF.Currency.Equals(Currency.Of("LUF")));
            Assert.True(LUF.Currency == Currency.Of("LUF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LUF"), Currency.Of("LUF")));
            Assert.False(Object.ReferenceEquals(LUF.Currency, Currency.Of("LUF")));

            Assert.True(LUL.Currency.Equals(Currency.Of("LUL")));
            Assert.True(LUL.Currency == Currency.Of("LUL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LUL"), Currency.Of("LUL")));
            Assert.False(Object.ReferenceEquals(LUL.Currency, Currency.Of("LUL")));

            Assert.True(LVL.Currency.Equals(Currency.Of("LVL")));
            Assert.True(LVL.Currency == Currency.Of("LVL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LVL"), Currency.Of("LVL")));
            Assert.False(Object.ReferenceEquals(LVL.Currency, Currency.Of("LVL")));

            Assert.True(LVR.Currency.Equals(Currency.Of("LVR")));
            Assert.True(LVR.Currency == Currency.Of("LVR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LVR"), Currency.Of("LVR")));
            Assert.False(Object.ReferenceEquals(LVR.Currency, Currency.Of("LVR")));

            Assert.True(LYD.Currency.Equals(Currency.Of("LYD")));
            Assert.True(LYD.Currency == Currency.Of("LYD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("LYD"), Currency.Of("LYD")));
            Assert.False(Object.ReferenceEquals(LYD.Currency, Currency.Of("LYD")));

            Assert.True(MAD.Currency.Equals(Currency.Of("MAD")));
            Assert.True(MAD.Currency == Currency.Of("MAD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MAD"), Currency.Of("MAD")));
            Assert.False(Object.ReferenceEquals(MAD.Currency, Currency.Of("MAD")));

            Assert.True(MAF.Currency.Equals(Currency.Of("MAF")));
            Assert.True(MAF.Currency == Currency.Of("MAF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MAF"), Currency.Of("MAF")));
            Assert.False(Object.ReferenceEquals(MAF.Currency, Currency.Of("MAF")));

            Assert.True(MDL.Currency.Equals(Currency.Of("MDL")));
            Assert.True(MDL.Currency == Currency.Of("MDL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MDL"), Currency.Of("MDL")));
            Assert.False(Object.ReferenceEquals(MDL.Currency, Currency.Of("MDL")));

            Assert.True(MGA.Currency.Equals(Currency.Of("MGA")));
            Assert.True(MGA.Currency == Currency.Of("MGA"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MGA"), Currency.Of("MGA")));
            Assert.False(Object.ReferenceEquals(MGA.Currency, Currency.Of("MGA")));

            Assert.True(MGF.Currency.Equals(Currency.Of("MGF")));
            Assert.True(MGF.Currency == Currency.Of("MGF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MGF"), Currency.Of("MGF")));
            Assert.False(Object.ReferenceEquals(MGF.Currency, Currency.Of("MGF")));

            Assert.True(MKD.Currency.Equals(Currency.Of("MKD")));
            Assert.True(MKD.Currency == Currency.Of("MKD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MKD"), Currency.Of("MKD")));
            Assert.False(Object.ReferenceEquals(MKD.Currency, Currency.Of("MKD")));

            Assert.True(MLF.Currency.Equals(Currency.Of("MLF")));
            Assert.True(MLF.Currency == Currency.Of("MLF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MLF"), Currency.Of("MLF")));
            Assert.False(Object.ReferenceEquals(MLF.Currency, Currency.Of("MLF")));

            Assert.True(MMK.Currency.Equals(Currency.Of("MMK")));
            Assert.True(MMK.Currency == Currency.Of("MMK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MMK"), Currency.Of("MMK")));
            Assert.False(Object.ReferenceEquals(MMK.Currency, Currency.Of("MMK")));

            Assert.True(MNT.Currency.Equals(Currency.Of("MNT")));
            Assert.True(MNT.Currency == Currency.Of("MNT"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MNT"), Currency.Of("MNT")));
            Assert.False(Object.ReferenceEquals(MNT.Currency, Currency.Of("MNT")));

            Assert.True(MOP.Currency.Equals(Currency.Of("MOP")));
            Assert.True(MOP.Currency == Currency.Of("MOP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MOP"), Currency.Of("MOP")));
            Assert.False(Object.ReferenceEquals(MOP.Currency, Currency.Of("MOP")));

            Assert.True(MRO.Currency.Equals(Currency.Of("MRO")));
            Assert.True(MRO.Currency == Currency.Of("MRO"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MRO"), Currency.Of("MRO")));
            Assert.False(Object.ReferenceEquals(MRO.Currency, Currency.Of("MRO")));

            Assert.True(MTL.Currency.Equals(Currency.Of("MTL")));
            Assert.True(MTL.Currency == Currency.Of("MTL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MTL"), Currency.Of("MTL")));
            Assert.False(Object.ReferenceEquals(MTL.Currency, Currency.Of("MTL")));

            Assert.True(MTP.Currency.Equals(Currency.Of("MTP")));
            Assert.True(MTP.Currency == Currency.Of("MTP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MTP"), Currency.Of("MTP")));
            Assert.False(Object.ReferenceEquals(MTP.Currency, Currency.Of("MTP")));

            Assert.True(MUR.Currency.Equals(Currency.Of("MUR")));
            Assert.True(MUR.Currency == Currency.Of("MUR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MUR"), Currency.Of("MUR")));
            Assert.False(Object.ReferenceEquals(MUR.Currency, Currency.Of("MUR")));

            Assert.True(MVQ.Currency.Equals(Currency.Of("MVQ")));
            Assert.True(MVQ.Currency == Currency.Of("MVQ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MVQ"), Currency.Of("MVQ")));
            Assert.False(Object.ReferenceEquals(MVQ.Currency, Currency.Of("MVQ")));

            Assert.True(MVR.Currency.Equals(Currency.Of("MVR")));
            Assert.True(MVR.Currency == Currency.Of("MVR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MVR"), Currency.Of("MVR")));
            Assert.False(Object.ReferenceEquals(MVR.Currency, Currency.Of("MVR")));

            Assert.True(MWK.Currency.Equals(Currency.Of("MWK")));
            Assert.True(MWK.Currency == Currency.Of("MWK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MWK"), Currency.Of("MWK")));
            Assert.False(Object.ReferenceEquals(MWK.Currency, Currency.Of("MWK")));

            Assert.True(MXN.Currency.Equals(Currency.Of("MXN")));
            Assert.True(MXN.Currency == Currency.Of("MXN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MXN"), Currency.Of("MXN")));
            Assert.False(Object.ReferenceEquals(MXN.Currency, Currency.Of("MXN")));

            Assert.True(MXP.Currency.Equals(Currency.Of("MXP")));
            Assert.True(MXP.Currency == Currency.Of("MXP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MXP"), Currency.Of("MXP")));
            Assert.False(Object.ReferenceEquals(MXP.Currency, Currency.Of("MXP")));

            Assert.True(MXV.Currency.Equals(Currency.Of("MXV")));
            Assert.True(MXV.Currency == Currency.Of("MXV"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MXV"), Currency.Of("MXV")));
            Assert.False(Object.ReferenceEquals(MXV.Currency, Currency.Of("MXV")));

            Assert.True(MYR.Currency.Equals(Currency.Of("MYR")));
            Assert.True(MYR.Currency == Currency.Of("MYR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MYR"), Currency.Of("MYR")));
            Assert.False(Object.ReferenceEquals(MYR.Currency, Currency.Of("MYR")));

            Assert.True(MZE.Currency.Equals(Currency.Of("MZE")));
            Assert.True(MZE.Currency == Currency.Of("MZE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MZE"), Currency.Of("MZE")));
            Assert.False(Object.ReferenceEquals(MZE.Currency, Currency.Of("MZE")));

            Assert.True(MZM.Currency.Equals(Currency.Of("MZM")));
            Assert.True(MZM.Currency == Currency.Of("MZM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MZM"), Currency.Of("MZM")));
            Assert.False(Object.ReferenceEquals(MZM.Currency, Currency.Of("MZM")));

            Assert.True(MZN.Currency.Equals(Currency.Of("MZN")));
            Assert.True(MZN.Currency == Currency.Of("MZN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("MZN"), Currency.Of("MZN")));
            Assert.False(Object.ReferenceEquals(MZN.Currency, Currency.Of("MZN")));

            Assert.True(NAD.Currency.Equals(Currency.Of("NAD")));
            Assert.True(NAD.Currency == Currency.Of("NAD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NAD"), Currency.Of("NAD")));
            Assert.False(Object.ReferenceEquals(NAD.Currency, Currency.Of("NAD")));

            Assert.True(NGN.Currency.Equals(Currency.Of("NGN")));
            Assert.True(NGN.Currency == Currency.Of("NGN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NGN"), Currency.Of("NGN")));
            Assert.False(Object.ReferenceEquals(NGN.Currency, Currency.Of("NGN")));

            Assert.True(NIC.Currency.Equals(Currency.Of("NIC")));
            Assert.True(NIC.Currency == Currency.Of("NIC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NIC"), Currency.Of("NIC")));
            Assert.False(Object.ReferenceEquals(NIC.Currency, Currency.Of("NIC")));

            Assert.True(NIO.Currency.Equals(Currency.Of("NIO")));
            Assert.True(NIO.Currency == Currency.Of("NIO"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NIO"), Currency.Of("NIO")));
            Assert.False(Object.ReferenceEquals(NIO.Currency, Currency.Of("NIO")));

            Assert.True(NLG.Currency.Equals(Currency.Of("NLG")));
            Assert.True(NLG.Currency == Currency.Of("NLG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NLG"), Currency.Of("NLG")));
            Assert.False(Object.ReferenceEquals(NLG.Currency, Currency.Of("NLG")));

            Assert.True(NOK.Currency.Equals(Currency.Of("NOK")));
            Assert.True(NOK.Currency == Currency.Of("NOK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NOK"), Currency.Of("NOK")));
            Assert.False(Object.ReferenceEquals(NOK.Currency, Currency.Of("NOK")));

            Assert.True(NPR.Currency.Equals(Currency.Of("NPR")));
            Assert.True(NPR.Currency == Currency.Of("NPR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NPR"), Currency.Of("NPR")));
            Assert.False(Object.ReferenceEquals(NPR.Currency, Currency.Of("NPR")));

            Assert.True(NZD.Currency.Equals(Currency.Of("NZD")));
            Assert.True(NZD.Currency == Currency.Of("NZD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("NZD"), Currency.Of("NZD")));
            Assert.False(Object.ReferenceEquals(NZD.Currency, Currency.Of("NZD")));

            Assert.True(OMR.Currency.Equals(Currency.Of("OMR")));
            Assert.True(OMR.Currency == Currency.Of("OMR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("OMR"), Currency.Of("OMR")));
            Assert.False(Object.ReferenceEquals(OMR.Currency, Currency.Of("OMR")));

            Assert.True(PAB.Currency.Equals(Currency.Of("PAB")));
            Assert.True(PAB.Currency == Currency.Of("PAB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PAB"), Currency.Of("PAB")));
            Assert.False(Object.ReferenceEquals(PAB.Currency, Currency.Of("PAB")));

            Assert.True(PEH.Currency.Equals(Currency.Of("PEH")));
            Assert.True(PEH.Currency == Currency.Of("PEH"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PEH"), Currency.Of("PEH")));
            Assert.False(Object.ReferenceEquals(PEH.Currency, Currency.Of("PEH")));

            Assert.True(PEI.Currency.Equals(Currency.Of("PEI")));
            Assert.True(PEI.Currency == Currency.Of("PEI"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PEI"), Currency.Of("PEI")));
            Assert.False(Object.ReferenceEquals(PEI.Currency, Currency.Of("PEI")));

            Assert.True(PEN.Currency.Equals(Currency.Of("PEN")));
            Assert.True(PEN.Currency == Currency.Of("PEN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PEN"), Currency.Of("PEN")));
            Assert.False(Object.ReferenceEquals(PEN.Currency, Currency.Of("PEN")));

            Assert.True(PES.Currency.Equals(Currency.Of("PES")));
            Assert.True(PES.Currency == Currency.Of("PES"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PES"), Currency.Of("PES")));
            Assert.False(Object.ReferenceEquals(PES.Currency, Currency.Of("PES")));

            Assert.True(PGK.Currency.Equals(Currency.Of("PGK")));
            Assert.True(PGK.Currency == Currency.Of("PGK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PGK"), Currency.Of("PGK")));
            Assert.False(Object.ReferenceEquals(PGK.Currency, Currency.Of("PGK")));

            Assert.True(PHP.Currency.Equals(Currency.Of("PHP")));
            Assert.True(PHP.Currency == Currency.Of("PHP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PHP"), Currency.Of("PHP")));
            Assert.False(Object.ReferenceEquals(PHP.Currency, Currency.Of("PHP")));

            Assert.True(PKR.Currency.Equals(Currency.Of("PKR")));
            Assert.True(PKR.Currency == Currency.Of("PKR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PKR"), Currency.Of("PKR")));
            Assert.False(Object.ReferenceEquals(PKR.Currency, Currency.Of("PKR")));

            Assert.True(PLN.Currency.Equals(Currency.Of("PLN")));
            Assert.True(PLN.Currency == Currency.Of("PLN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PLN"), Currency.Of("PLN")));
            Assert.False(Object.ReferenceEquals(PLN.Currency, Currency.Of("PLN")));

            Assert.True(PLZ.Currency.Equals(Currency.Of("PLZ")));
            Assert.True(PLZ.Currency == Currency.Of("PLZ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PLZ"), Currency.Of("PLZ")));
            Assert.False(Object.ReferenceEquals(PLZ.Currency, Currency.Of("PLZ")));

            Assert.True(PTE.Currency.Equals(Currency.Of("PTE")));
            Assert.True(PTE.Currency == Currency.Of("PTE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PTE"), Currency.Of("PTE")));
            Assert.False(Object.ReferenceEquals(PTE.Currency, Currency.Of("PTE")));

            Assert.True(PYG.Currency.Equals(Currency.Of("PYG")));
            Assert.True(PYG.Currency == Currency.Of("PYG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("PYG"), Currency.Of("PYG")));
            Assert.False(Object.ReferenceEquals(PYG.Currency, Currency.Of("PYG")));

            Assert.True(QAR.Currency.Equals(Currency.Of("QAR")));
            Assert.True(QAR.Currency == Currency.Of("QAR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("QAR"), Currency.Of("QAR")));
            Assert.False(Object.ReferenceEquals(QAR.Currency, Currency.Of("QAR")));

            Assert.True(RHD.Currency.Equals(Currency.Of("RHD")));
            Assert.True(RHD.Currency == Currency.Of("RHD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("RHD"), Currency.Of("RHD")));
            Assert.False(Object.ReferenceEquals(RHD.Currency, Currency.Of("RHD")));

            Assert.True(ROK.Currency.Equals(Currency.Of("ROK")));
            Assert.True(ROK.Currency == Currency.Of("ROK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ROK"), Currency.Of("ROK")));
            Assert.False(Object.ReferenceEquals(ROK.Currency, Currency.Of("ROK")));

            Assert.True(ROL.Currency.Equals(Currency.Of("ROL")));
            Assert.True(ROL.Currency == Currency.Of("ROL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ROL"), Currency.Of("ROL")));
            Assert.False(Object.ReferenceEquals(ROL.Currency, Currency.Of("ROL")));

            Assert.True(RON.Currency.Equals(Currency.Of("RON")));
            Assert.True(RON.Currency == Currency.Of("RON"));
            Assert.True(Object.ReferenceEquals(Currency.Of("RON"), Currency.Of("RON")));
            Assert.False(Object.ReferenceEquals(RON.Currency, Currency.Of("RON")));

            Assert.True(RSD.Currency.Equals(Currency.Of("RSD")));
            Assert.True(RSD.Currency == Currency.Of("RSD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("RSD"), Currency.Of("RSD")));
            Assert.False(Object.ReferenceEquals(RSD.Currency, Currency.Of("RSD")));

            Assert.True(RUB.Currency.Equals(Currency.Of("RUB")));
            Assert.True(RUB.Currency == Currency.Of("RUB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("RUB"), Currency.Of("RUB")));
            Assert.False(Object.ReferenceEquals(RUB.Currency, Currency.Of("RUB")));

            Assert.True(RUR.Currency.Equals(Currency.Of("RUR")));
            Assert.True(RUR.Currency == Currency.Of("RUR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("RUR"), Currency.Of("RUR")));
            Assert.False(Object.ReferenceEquals(RUR.Currency, Currency.Of("RUR")));

            Assert.True(RWF.Currency.Equals(Currency.Of("RWF")));
            Assert.True(RWF.Currency == Currency.Of("RWF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("RWF"), Currency.Of("RWF")));
            Assert.False(Object.ReferenceEquals(RWF.Currency, Currency.Of("RWF")));

            Assert.True(SAR.Currency.Equals(Currency.Of("SAR")));
            Assert.True(SAR.Currency == Currency.Of("SAR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SAR"), Currency.Of("SAR")));
            Assert.False(Object.ReferenceEquals(SAR.Currency, Currency.Of("SAR")));

            Assert.True(SBD.Currency.Equals(Currency.Of("SBD")));
            Assert.True(SBD.Currency == Currency.Of("SBD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SBD"), Currency.Of("SBD")));
            Assert.False(Object.ReferenceEquals(SBD.Currency, Currency.Of("SBD")));

            Assert.True(SCR.Currency.Equals(Currency.Of("SCR")));
            Assert.True(SCR.Currency == Currency.Of("SCR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SCR"), Currency.Of("SCR")));
            Assert.False(Object.ReferenceEquals(SCR.Currency, Currency.Of("SCR")));

            Assert.True(SDD.Currency.Equals(Currency.Of("SDD")));
            Assert.True(SDD.Currency == Currency.Of("SDD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SDD"), Currency.Of("SDD")));
            Assert.False(Object.ReferenceEquals(SDD.Currency, Currency.Of("SDD")));

            Assert.True(SDG.Currency.Equals(Currency.Of("SDG")));
            Assert.True(SDG.Currency == Currency.Of("SDG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SDG"), Currency.Of("SDG")));
            Assert.False(Object.ReferenceEquals(SDG.Currency, Currency.Of("SDG")));

            Assert.True(SDP.Currency.Equals(Currency.Of("SDP")));
            Assert.True(SDP.Currency == Currency.Of("SDP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SDP"), Currency.Of("SDP")));
            Assert.False(Object.ReferenceEquals(SDP.Currency, Currency.Of("SDP")));

            Assert.True(SEK.Currency.Equals(Currency.Of("SEK")));
            Assert.True(SEK.Currency == Currency.Of("SEK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SEK"), Currency.Of("SEK")));
            Assert.False(Object.ReferenceEquals(SEK.Currency, Currency.Of("SEK")));

            Assert.True(SGD.Currency.Equals(Currency.Of("SGD")));
            Assert.True(SGD.Currency == Currency.Of("SGD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SGD"), Currency.Of("SGD")));
            Assert.False(Object.ReferenceEquals(SGD.Currency, Currency.Of("SGD")));

            Assert.True(SHP.Currency.Equals(Currency.Of("SHP")));
            Assert.True(SHP.Currency == Currency.Of("SHP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SHP"), Currency.Of("SHP")));
            Assert.False(Object.ReferenceEquals(SHP.Currency, Currency.Of("SHP")));

            Assert.True(SIT.Currency.Equals(Currency.Of("SIT")));
            Assert.True(SIT.Currency == Currency.Of("SIT"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SIT"), Currency.Of("SIT")));
            Assert.False(Object.ReferenceEquals(SIT.Currency, Currency.Of("SIT")));

            Assert.True(SKK.Currency.Equals(Currency.Of("SKK")));
            Assert.True(SKK.Currency == Currency.Of("SKK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SKK"), Currency.Of("SKK")));
            Assert.False(Object.ReferenceEquals(SKK.Currency, Currency.Of("SKK")));

            Assert.True(SLL.Currency.Equals(Currency.Of("SLL")));
            Assert.True(SLL.Currency == Currency.Of("SLL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SLL"), Currency.Of("SLL")));
            Assert.False(Object.ReferenceEquals(SLL.Currency, Currency.Of("SLL")));

            Assert.True(SOS.Currency.Equals(Currency.Of("SOS")));
            Assert.True(SOS.Currency == Currency.Of("SOS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SOS"), Currency.Of("SOS")));
            Assert.False(Object.ReferenceEquals(SOS.Currency, Currency.Of("SOS")));

            Assert.True(SRD.Currency.Equals(Currency.Of("SRD")));
            Assert.True(SRD.Currency == Currency.Of("SRD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SRD"), Currency.Of("SRD")));
            Assert.False(Object.ReferenceEquals(SRD.Currency, Currency.Of("SRD")));

            Assert.True(SRG.Currency.Equals(Currency.Of("SRG")));
            Assert.True(SRG.Currency == Currency.Of("SRG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SRG"), Currency.Of("SRG")));
            Assert.False(Object.ReferenceEquals(SRG.Currency, Currency.Of("SRG")));

            Assert.True(SSP.Currency.Equals(Currency.Of("SSP")));
            Assert.True(SSP.Currency == Currency.Of("SSP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SSP"), Currency.Of("SSP")));
            Assert.False(Object.ReferenceEquals(SSP.Currency, Currency.Of("SSP")));

            Assert.True(STD.Currency.Equals(Currency.Of("STD")));
            Assert.True(STD.Currency == Currency.Of("STD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("STD"), Currency.Of("STD")));
            Assert.False(Object.ReferenceEquals(STD.Currency, Currency.Of("STD")));

            Assert.True(SUR.Currency.Equals(Currency.Of("SUR")));
            Assert.True(SUR.Currency == Currency.Of("SUR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SUR"), Currency.Of("SUR")));
            Assert.False(Object.ReferenceEquals(SUR.Currency, Currency.Of("SUR")));

            Assert.True(SVC.Currency.Equals(Currency.Of("SVC")));
            Assert.True(SVC.Currency == Currency.Of("SVC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SVC"), Currency.Of("SVC")));
            Assert.False(Object.ReferenceEquals(SVC.Currency, Currency.Of("SVC")));

            Assert.True(SYP.Currency.Equals(Currency.Of("SYP")));
            Assert.True(SYP.Currency == Currency.Of("SYP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SYP"), Currency.Of("SYP")));
            Assert.False(Object.ReferenceEquals(SYP.Currency, Currency.Of("SYP")));

            Assert.True(SZL.Currency.Equals(Currency.Of("SZL")));
            Assert.True(SZL.Currency == Currency.Of("SZL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("SZL"), Currency.Of("SZL")));
            Assert.False(Object.ReferenceEquals(SZL.Currency, Currency.Of("SZL")));

            Assert.True(THB.Currency.Equals(Currency.Of("THB")));
            Assert.True(THB.Currency == Currency.Of("THB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("THB"), Currency.Of("THB")));
            Assert.False(Object.ReferenceEquals(THB.Currency, Currency.Of("THB")));

            Assert.True(TJR.Currency.Equals(Currency.Of("TJR")));
            Assert.True(TJR.Currency == Currency.Of("TJR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TJR"), Currency.Of("TJR")));
            Assert.False(Object.ReferenceEquals(TJR.Currency, Currency.Of("TJR")));

            Assert.True(TJS.Currency.Equals(Currency.Of("TJS")));
            Assert.True(TJS.Currency == Currency.Of("TJS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TJS"), Currency.Of("TJS")));
            Assert.False(Object.ReferenceEquals(TJS.Currency, Currency.Of("TJS")));

            Assert.True(TMM.Currency.Equals(Currency.Of("TMM")));
            Assert.True(TMM.Currency == Currency.Of("TMM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TMM"), Currency.Of("TMM")));
            Assert.False(Object.ReferenceEquals(TMM.Currency, Currency.Of("TMM")));

            Assert.True(TMT.Currency.Equals(Currency.Of("TMT")));
            Assert.True(TMT.Currency == Currency.Of("TMT"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TMT"), Currency.Of("TMT")));
            Assert.False(Object.ReferenceEquals(TMT.Currency, Currency.Of("TMT")));

            Assert.True(TND.Currency.Equals(Currency.Of("TND")));
            Assert.True(TND.Currency == Currency.Of("TND"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TND"), Currency.Of("TND")));
            Assert.False(Object.ReferenceEquals(TND.Currency, Currency.Of("TND")));

            Assert.True(TOP.Currency.Equals(Currency.Of("TOP")));
            Assert.True(TOP.Currency == Currency.Of("TOP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TOP"), Currency.Of("TOP")));
            Assert.False(Object.ReferenceEquals(TOP.Currency, Currency.Of("TOP")));

            Assert.True(TPE.Currency.Equals(Currency.Of("TPE")));
            Assert.True(TPE.Currency == Currency.Of("TPE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TPE"), Currency.Of("TPE")));
            Assert.False(Object.ReferenceEquals(TPE.Currency, Currency.Of("TPE")));

            Assert.True(TRL.Currency.Equals(Currency.Of("TRL")));
            Assert.True(TRL.Currency == Currency.Of("TRL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TRL"), Currency.Of("TRL")));
            Assert.False(Object.ReferenceEquals(TRL.Currency, Currency.Of("TRL")));

            Assert.True(TRY.Currency.Equals(Currency.Of("TRY")));
            Assert.True(TRY.Currency == Currency.Of("TRY"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TRY"), Currency.Of("TRY")));
            Assert.False(Object.ReferenceEquals(TRY.Currency, Currency.Of("TRY")));

            Assert.True(TTD.Currency.Equals(Currency.Of("TTD")));
            Assert.True(TTD.Currency == Currency.Of("TTD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TTD"), Currency.Of("TTD")));
            Assert.False(Object.ReferenceEquals(TTD.Currency, Currency.Of("TTD")));

            Assert.True(TWD.Currency.Equals(Currency.Of("TWD")));
            Assert.True(TWD.Currency == Currency.Of("TWD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TWD"), Currency.Of("TWD")));
            Assert.False(Object.ReferenceEquals(TWD.Currency, Currency.Of("TWD")));

            Assert.True(TZS.Currency.Equals(Currency.Of("TZS")));
            Assert.True(TZS.Currency == Currency.Of("TZS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("TZS"), Currency.Of("TZS")));
            Assert.False(Object.ReferenceEquals(TZS.Currency, Currency.Of("TZS")));

            Assert.True(UAH.Currency.Equals(Currency.Of("UAH")));
            Assert.True(UAH.Currency == Currency.Of("UAH"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UAH"), Currency.Of("UAH")));
            Assert.False(Object.ReferenceEquals(UAH.Currency, Currency.Of("UAH")));

            Assert.True(UAK.Currency.Equals(Currency.Of("UAK")));
            Assert.True(UAK.Currency == Currency.Of("UAK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UAK"), Currency.Of("UAK")));
            Assert.False(Object.ReferenceEquals(UAK.Currency, Currency.Of("UAK")));

            Assert.True(UGS.Currency.Equals(Currency.Of("UGS")));
            Assert.True(UGS.Currency == Currency.Of("UGS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UGS"), Currency.Of("UGS")));
            Assert.False(Object.ReferenceEquals(UGS.Currency, Currency.Of("UGS")));

            Assert.True(UGW.Currency.Equals(Currency.Of("UGW")));
            Assert.True(UGW.Currency == Currency.Of("UGW"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UGW"), Currency.Of("UGW")));
            Assert.False(Object.ReferenceEquals(UGW.Currency, Currency.Of("UGW")));

            Assert.True(UGX.Currency.Equals(Currency.Of("UGX")));
            Assert.True(UGX.Currency == Currency.Of("UGX"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UGX"), Currency.Of("UGX")));
            Assert.False(Object.ReferenceEquals(UGX.Currency, Currency.Of("UGX")));

            Assert.True(USD.Currency.Equals(Currency.Of("USD")));
            Assert.True(USD.Currency == Currency.Of("USD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("USD"), Currency.Of("USD")));
            Assert.False(Object.ReferenceEquals(USD.Currency, Currency.Of("USD")));

            Assert.True(USN.Currency.Equals(Currency.Of("USN")));
            Assert.True(USN.Currency == Currency.Of("USN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("USN"), Currency.Of("USN")));
            Assert.False(Object.ReferenceEquals(USN.Currency, Currency.Of("USN")));

            Assert.True(USS.Currency.Equals(Currency.Of("USS")));
            Assert.True(USS.Currency == Currency.Of("USS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("USS"), Currency.Of("USS")));
            Assert.False(Object.ReferenceEquals(USS.Currency, Currency.Of("USS")));

            Assert.True(UYI.Currency.Equals(Currency.Of("UYI")));
            Assert.True(UYI.Currency == Currency.Of("UYI"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UYI"), Currency.Of("UYI")));
            Assert.False(Object.ReferenceEquals(UYI.Currency, Currency.Of("UYI")));

            Assert.True(UYN.Currency.Equals(Currency.Of("UYN")));
            Assert.True(UYN.Currency == Currency.Of("UYN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UYN"), Currency.Of("UYN")));
            Assert.False(Object.ReferenceEquals(UYN.Currency, Currency.Of("UYN")));

            Assert.True(UYP.Currency.Equals(Currency.Of("UYP")));
            Assert.True(UYP.Currency == Currency.Of("UYP"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UYP"), Currency.Of("UYP")));
            Assert.False(Object.ReferenceEquals(UYP.Currency, Currency.Of("UYP")));

            Assert.True(UYU.Currency.Equals(Currency.Of("UYU")));
            Assert.True(UYU.Currency == Currency.Of("UYU"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UYU"), Currency.Of("UYU")));
            Assert.False(Object.ReferenceEquals(UYU.Currency, Currency.Of("UYU")));

            Assert.True(UZS.Currency.Equals(Currency.Of("UZS")));
            Assert.True(UZS.Currency == Currency.Of("UZS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("UZS"), Currency.Of("UZS")));
            Assert.False(Object.ReferenceEquals(UZS.Currency, Currency.Of("UZS")));

            Assert.True(VEB.Currency.Equals(Currency.Of("VEB")));
            Assert.True(VEB.Currency == Currency.Of("VEB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("VEB"), Currency.Of("VEB")));
            Assert.False(Object.ReferenceEquals(VEB.Currency, Currency.Of("VEB")));

            Assert.True(VEF.Currency.Equals(Currency.Of("VEF")));
            Assert.True(VEF.Currency == Currency.Of("VEF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("VEF"), Currency.Of("VEF")));
            Assert.False(Object.ReferenceEquals(VEF.Currency, Currency.Of("VEF")));

            Assert.True(VNC.Currency.Equals(Currency.Of("VNC")));
            Assert.True(VNC.Currency == Currency.Of("VNC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("VNC"), Currency.Of("VNC")));
            Assert.False(Object.ReferenceEquals(VNC.Currency, Currency.Of("VNC")));

            Assert.True(VND.Currency.Equals(Currency.Of("VND")));
            Assert.True(VND.Currency == Currency.Of("VND"));
            Assert.True(Object.ReferenceEquals(Currency.Of("VND"), Currency.Of("VND")));
            Assert.False(Object.ReferenceEquals(VND.Currency, Currency.Of("VND")));

            Assert.True(VUV.Currency.Equals(Currency.Of("VUV")));
            Assert.True(VUV.Currency == Currency.Of("VUV"));
            Assert.True(Object.ReferenceEquals(Currency.Of("VUV"), Currency.Of("VUV")));
            Assert.False(Object.ReferenceEquals(VUV.Currency, Currency.Of("VUV")));

            Assert.True(WST.Currency.Equals(Currency.Of("WST")));
            Assert.True(WST.Currency == Currency.Of("WST"));
            Assert.True(Object.ReferenceEquals(Currency.Of("WST"), Currency.Of("WST")));
            Assert.False(Object.ReferenceEquals(WST.Currency, Currency.Of("WST")));

            Assert.True(XAF.Currency.Equals(Currency.Of("XAF")));
            Assert.True(XAF.Currency == Currency.Of("XAF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XAF"), Currency.Of("XAF")));
            Assert.False(Object.ReferenceEquals(XAF.Currency, Currency.Of("XAF")));

            Assert.True(XAG.Currency.Equals(Currency.Of("XAG")));
            Assert.True(XAG.Currency == Currency.Of("XAG"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XAG"), Currency.Of("XAG")));
            Assert.False(Object.ReferenceEquals(XAG.Currency, Currency.Of("XAG")));

            Assert.True(XAU.Currency.Equals(Currency.Of("XAU")));
            Assert.True(XAU.Currency == Currency.Of("XAU"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XAU"), Currency.Of("XAU")));
            Assert.False(Object.ReferenceEquals(XAU.Currency, Currency.Of("XAU")));

            Assert.True(XBA.Currency.Equals(Currency.Of("XBA")));
            Assert.True(XBA.Currency == Currency.Of("XBA"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XBA"), Currency.Of("XBA")));
            Assert.False(Object.ReferenceEquals(XBA.Currency, Currency.Of("XBA")));

            Assert.True(XBB.Currency.Equals(Currency.Of("XBB")));
            Assert.True(XBB.Currency == Currency.Of("XBB"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XBB"), Currency.Of("XBB")));
            Assert.False(Object.ReferenceEquals(XBB.Currency, Currency.Of("XBB")));

            Assert.True(XBC.Currency.Equals(Currency.Of("XBC")));
            Assert.True(XBC.Currency == Currency.Of("XBC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XBC"), Currency.Of("XBC")));
            Assert.False(Object.ReferenceEquals(XBC.Currency, Currency.Of("XBC")));

            Assert.True(XBD.Currency.Equals(Currency.Of("XBD")));
            Assert.True(XBD.Currency == Currency.Of("XBD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XBD"), Currency.Of("XBD")));
            Assert.False(Object.ReferenceEquals(XBD.Currency, Currency.Of("XBD")));

            Assert.True(XCD.Currency.Equals(Currency.Of("XCD")));
            Assert.True(XCD.Currency == Currency.Of("XCD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XCD"), Currency.Of("XCD")));
            Assert.False(Object.ReferenceEquals(XCD.Currency, Currency.Of("XCD")));

            Assert.True(XDR.Currency.Equals(Currency.Of("XDR")));
            Assert.True(XDR.Currency == Currency.Of("XDR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XDR"), Currency.Of("XDR")));
            Assert.False(Object.ReferenceEquals(XDR.Currency, Currency.Of("XDR")));

            Assert.True(XEU.Currency.Equals(Currency.Of("XEU")));
            Assert.True(XEU.Currency == Currency.Of("XEU"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XEU"), Currency.Of("XEU")));
            Assert.False(Object.ReferenceEquals(XEU.Currency, Currency.Of("XEU")));

            Assert.True(XFO.Currency.Equals(Currency.Of("XFO")));
            Assert.True(XFO.Currency == Currency.Of("XFO"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XFO"), Currency.Of("XFO")));
            Assert.False(Object.ReferenceEquals(XFO.Currency, Currency.Of("XFO")));

            Assert.True(XFU.Currency.Equals(Currency.Of("XFU")));
            Assert.True(XFU.Currency == Currency.Of("XFU"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XFU"), Currency.Of("XFU")));
            Assert.False(Object.ReferenceEquals(XFU.Currency, Currency.Of("XFU")));

            Assert.True(XOF.Currency.Equals(Currency.Of("XOF")));
            Assert.True(XOF.Currency == Currency.Of("XOF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XOF"), Currency.Of("XOF")));
            Assert.False(Object.ReferenceEquals(XOF.Currency, Currency.Of("XOF")));

            Assert.True(XPD.Currency.Equals(Currency.Of("XPD")));
            Assert.True(XPD.Currency == Currency.Of("XPD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XPD"), Currency.Of("XPD")));
            Assert.False(Object.ReferenceEquals(XPD.Currency, Currency.Of("XPD")));

            Assert.True(XPF.Currency.Equals(Currency.Of("XPF")));
            Assert.True(XPF.Currency == Currency.Of("XPF"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XPF"), Currency.Of("XPF")));
            Assert.False(Object.ReferenceEquals(XPF.Currency, Currency.Of("XPF")));

            Assert.True(XPT.Currency.Equals(Currency.Of("XPT")));
            Assert.True(XPT.Currency == Currency.Of("XPT"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XPT"), Currency.Of("XPT")));
            Assert.False(Object.ReferenceEquals(XPT.Currency, Currency.Of("XPT")));

            Assert.True(XRE.Currency.Equals(Currency.Of("XRE")));
            Assert.True(XRE.Currency == Currency.Of("XRE"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XRE"), Currency.Of("XRE")));
            Assert.False(Object.ReferenceEquals(XRE.Currency, Currency.Of("XRE")));

            Assert.True(XSU.Currency.Equals(Currency.Of("XSU")));
            Assert.True(XSU.Currency == Currency.Of("XSU"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XSU"), Currency.Of("XSU")));
            Assert.False(Object.ReferenceEquals(XSU.Currency, Currency.Of("XSU")));

            Assert.True(XTS.Currency.Equals(Currency.Of("XTS")));
            Assert.True(XTS.Currency == Currency.Of("XTS"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XTS"), Currency.Of("XTS")));
            Assert.False(Object.ReferenceEquals(XTS.Currency, Currency.Of("XTS")));

            Assert.True(XUA.Currency.Equals(Currency.Of("XUA")));
            Assert.True(XUA.Currency == Currency.Of("XUA"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XUA"), Currency.Of("XUA")));
            Assert.False(Object.ReferenceEquals(XUA.Currency, Currency.Of("XUA")));

            Assert.True(XXX.Currency.Equals(Currency.Of("XXX")));
            Assert.True(XXX.Currency == Currency.Of("XXX"));
            Assert.True(Object.ReferenceEquals(Currency.Of("XXX"), Currency.Of("XXX")));
            Assert.False(Object.ReferenceEquals(XXX.Currency, Currency.Of("XXX")));

            Assert.True(YDD.Currency.Equals(Currency.Of("YDD")));
            Assert.True(YDD.Currency == Currency.Of("YDD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("YDD"), Currency.Of("YDD")));
            Assert.False(Object.ReferenceEquals(YDD.Currency, Currency.Of("YDD")));

            Assert.True(YER.Currency.Equals(Currency.Of("YER")));
            Assert.True(YER.Currency == Currency.Of("YER"));
            Assert.True(Object.ReferenceEquals(Currency.Of("YER"), Currency.Of("YER")));
            Assert.False(Object.ReferenceEquals(YER.Currency, Currency.Of("YER")));

            Assert.True(YUD.Currency.Equals(Currency.Of("YUD")));
            Assert.True(YUD.Currency == Currency.Of("YUD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("YUD"), Currency.Of("YUD")));
            Assert.False(Object.ReferenceEquals(YUD.Currency, Currency.Of("YUD")));

            Assert.True(YUM.Currency.Equals(Currency.Of("YUM")));
            Assert.True(YUM.Currency == Currency.Of("YUM"));
            Assert.True(Object.ReferenceEquals(Currency.Of("YUM"), Currency.Of("YUM")));
            Assert.False(Object.ReferenceEquals(YUM.Currency, Currency.Of("YUM")));

            Assert.True(YUN.Currency.Equals(Currency.Of("YUN")));
            Assert.True(YUN.Currency == Currency.Of("YUN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("YUN"), Currency.Of("YUN")));
            Assert.False(Object.ReferenceEquals(YUN.Currency, Currency.Of("YUN")));

            Assert.True(ZAL.Currency.Equals(Currency.Of("ZAL")));
            Assert.True(ZAL.Currency == Currency.Of("ZAL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZAL"), Currency.Of("ZAL")));
            Assert.False(Object.ReferenceEquals(ZAL.Currency, Currency.Of("ZAL")));

            Assert.True(ZAR.Currency.Equals(Currency.Of("ZAR")));
            Assert.True(ZAR.Currency == Currency.Of("ZAR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZAR"), Currency.Of("ZAR")));
            Assert.False(Object.ReferenceEquals(ZAR.Currency, Currency.Of("ZAR")));

            Assert.True(ZMK.Currency.Equals(Currency.Of("ZMK")));
            Assert.True(ZMK.Currency == Currency.Of("ZMK"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZMK"), Currency.Of("ZMK")));
            Assert.False(Object.ReferenceEquals(ZMK.Currency, Currency.Of("ZMK")));

            Assert.True(ZMW.Currency.Equals(Currency.Of("ZMW")));
            Assert.True(ZMW.Currency == Currency.Of("ZMW"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZMW"), Currency.Of("ZMW")));
            Assert.False(Object.ReferenceEquals(ZMW.Currency, Currency.Of("ZMW")));

            Assert.True(ZRN.Currency.Equals(Currency.Of("ZRN")));
            Assert.True(ZRN.Currency == Currency.Of("ZRN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZRN"), Currency.Of("ZRN")));
            Assert.False(Object.ReferenceEquals(ZRN.Currency, Currency.Of("ZRN")));

            Assert.True(ZRZ.Currency.Equals(Currency.Of("ZRZ")));
            Assert.True(ZRZ.Currency == Currency.Of("ZRZ"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZRZ"), Currency.Of("ZRZ")));
            Assert.False(Object.ReferenceEquals(ZRZ.Currency, Currency.Of("ZRZ")));

            Assert.True(ZWC.Currency.Equals(Currency.Of("ZWC")));
            Assert.True(ZWC.Currency == Currency.Of("ZWC"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZWC"), Currency.Of("ZWC")));
            Assert.False(Object.ReferenceEquals(ZWC.Currency, Currency.Of("ZWC")));

            Assert.True(ZWD.Currency.Equals(Currency.Of("ZWD")));
            Assert.True(ZWD.Currency == Currency.Of("ZWD"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZWD"), Currency.Of("ZWD")));
            Assert.False(Object.ReferenceEquals(ZWD.Currency, Currency.Of("ZWD")));

            Assert.True(ZWL.Currency.Equals(Currency.Of("ZWL")));
            Assert.True(ZWL.Currency == Currency.Of("ZWL"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZWL"), Currency.Of("ZWL")));
            Assert.False(Object.ReferenceEquals(ZWL.Currency, Currency.Of("ZWL")));

            Assert.True(ZWN.Currency.Equals(Currency.Of("ZWN")));
            Assert.True(ZWN.Currency == Currency.Of("ZWN"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZWN"), Currency.Of("ZWN")));
            Assert.False(Object.ReferenceEquals(ZWN.Currency, Currency.Of("ZWN")));

            Assert.True(ZWR.Currency.Equals(Currency.Of("ZWR")));
            Assert.True(ZWR.Currency == Currency.Of("ZWR"));
            Assert.True(Object.ReferenceEquals(Currency.Of("ZWR"), Currency.Of("ZWR")));
            Assert.False(Object.ReferenceEquals(ZWR.Currency, Currency.Of("ZWR")));

        }
    }
}