namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public static class Int64EncoderFacts
    {
        public static IEnumerable<object[]> Base58SampleData
        {
            get
            {
                yield return new object[] { String.Empty, 0 };
                yield return new object[] { "NQm6nKp8qFC", Int64.MaxValue };
            }
        }

        /// <summary>
        /// Test data borrowed from the Encode-Base58 Perl module.
        /// </summary>
        public static IEnumerable<object[]> FlickrBase58SampleData
        {
            get
            {
                #region Data
                yield return new object[] { String.Empty, 0 };
                yield return new object[] { "npL6MjP8Qfc", Int64.MaxValue };
                yield return new object[] { "6hKMCS", 3471391110 };
                yield return new object[] { "6hDrmR", 3470152229 };
                yield return new object[] { "6hHHZB", 3470988633 };
                yield return new object[] { "6hHKum", 3470993664 };
                yield return new object[] { "6hLgFW", 3471485480 };
                yield return new object[] { "6hBRKR", 3469844075 };
                yield return new object[] { "6hGRTd", 3470820062 };
                yield return new object[] { "6hCuie", 3469966999 };
                yield return new object[] { "6hJuXN", 3471139908 };
                yield return new object[] { "6hJsyS", 3471131850 };
                yield return new object[] { "6hFWFb", 3470641072 };
                yield return new object[] { "6hENdZ", 3470417529 };
                yield return new object[] { "6hEJqg", 3470404727 };
                yield return new object[] { "6hGNaq", 3470807546 };
                yield return new object[] { "6hDRoZ", 3470233089 };
                yield return new object[] { "6hKkP9", 3471304242 };
                yield return new object[] { "6hHVZ3", 3471028968 };
                yield return new object[] { "6hNcfE", 3471860782 };
                yield return new object[] { "6hJBqs", 3471161638 };
                yield return new object[] { "6hCPyc", 3470031783 };
                yield return new object[] { "6hJNrC", 3471198710 };
                yield return new object[] { "6hKmkd", 3471305986 };
                yield return new object[] { "6hFUYs", 3470635346 };
                yield return new object[] { "6hK6UC", 3471257464 };
                yield return new object[] { "6hBmiv", 3469744991 };
                yield return new object[] { "6hKex1", 3471283122 };
                yield return new object[] { "6hFHQj", 3470597870 };
                yield return new object[] { "6hCA2n", 3469986263 };
                yield return new object[] { "6hBTgt", 3469849157 };
                yield return new object[] { "6hHEss", 3470976734 };
                yield return new object[] { "6hLows", 3471508478 };
                yield return new object[] { "6hD95z", 3470094097 };
                yield return new object[] { "6hKjcq", 3471298806 };
                yield return new object[] { "6hGEbd", 3470780680 };
                yield return new object[] { "6hKSNS", 3471408510 };
                yield return new object[] { "6hG8hv", 3470676761 };
                yield return new object[] { "6hEmj6", 3470330361 };
                yield return new object[] { "6hGjpn", 3470714163 };
                yield return new object[] { "6hEsUr", 3470352537 };
                yield return new object[] { "6hJEhy", 3471171272 };
                yield return new object[] { "6hKBHn", 3471357731 };
                yield return new object[] { "6hG3gi", 3470659871 };
                yield return new object[] { "6hFJTT", 3470601441 };
                yield return new object[] { "6hLZDs", 3471626624 };
                yield return new object[] { "6hGdL7", 3470695182 };
                yield return new object[] { "6hBpi4", 3469755057 };
                yield return new object[] { "6hEuFV", 3470358539 };
                yield return new object[] { "6hGVw1", 3470832288 };
                yield return new object[] { "6hLdm1", 3471474232 };
                yield return new object[] { "6hFcCK", 3470496279 };
                yield return new object[] { "6hDZmR", 3470259877 };
                yield return new object[] { "6hG8iX", 3470676845 };
                yield return new object[] { "6hFZZL", 3470652242 };
                yield return new object[] { "6hJ79u", 3471063156 };
                yield return new object[] { "6hMsrS", 3471716780 };
                yield return new object[] { "6hGH3G", 3470790336 };
                yield return new object[] { "6hKqD3", 3471320476 };
                yield return new object[] { "6hKxEY", 3471344136 };
                yield return new object[] { "6hHVF1", 3471027922 };
                yield return new object[] { "6hKDSQ", 3471365008 };
                yield return new object[] { "6hKwHs", 3471340916 };
                yield return new object[] { "6hH4s6", 3470858973 };
                yield return new object[] { "6hGmKB", 3470722065 };
                yield return new object[] { "6hEzdi", 3470373757 };
                yield return new object[] { "6hJQwJ", 3471205734 };
                yield return new object[] { "6hHd6a", 3470888035 };
                yield return new object[] { "6hH1j3", 3470848414 };
                yield return new object[] { "6hNP1u", 3471981064 };
                yield return new object[] { "6hFWge", 3470639683 };
                yield return new object[] { "6hFpmP", 3470535723 };
                yield return new object[] { "6hCgQZ", 3469925109 };
                yield return new object[] { "6hFJSm", 3470601352 };
                yield return new object[] { "6hEd9v", 3470302893 };
                yield return new object[] { "6hDwuF", 3470169503 };
                yield return new object[] { "6hCVSX", 3470053055 };
                yield return new object[] { "6hCUgr", 3470047631 };
                yield return new object[] { "6hEsqR", 3470350937 };
                yield return new object[] { "6hBmKg", 3469746485 };
                yield return new object[] { "6hDvUx", 3470167523 };
                yield return new object[] { "6hJUi7", 3471218400 };
                yield return new object[] { "6hF39e", 3470464349 };
                yield return new object[] { "6hH43K", 3470857619 };
                yield return new object[] { "6hGSC5", 3470822548 };
                yield return new object[] { "6hFz1s", 3470568182 };
                yield return new object[] { "6hFaKZ", 3470489971 };
                yield return new object[] { "6hD65K", 3470084015 };
                yield return new object[] { "6hBAVk", 3469794165 };
                yield return new object[] { "6hLkWA", 3471499786 };
                yield return new object[] { "6hHi7q", 3470904928 };
                yield return new object[] { "6hHdDF", 3470889921 };
                yield return new object[] { "6hHcCo", 3470886482 };
                yield return new object[] { "6hGQQf", 3470816526 };
                yield return new object[] { "6hLAfo", 3471547914 };
                yield return new object[] { "6hBDEV", 3469803421 };
                yield return new object[] { "6hL4BE", 3471444864 };
                yield return new object[] { "6hL2TT", 3471439077 };
                yield return new object[] { "6hKcxb", 3471276404 };
                yield return new object[] { "6hD1vg", 3470068617 };
                yield return new object[] { "6hLtTT", 3471526541 };
                yield return new object[] { "6hHXtw", 3471033984 };
                yield return new object[] { "6hHCQj", 3470971274 };
                yield return new object[] { "6hFrXx", 3470544465 };
                yield return new object[] { "6hMVkJ", 3471807252 };
                yield return new object[] { "6hDv6V", 3470164819 };
                yield return new object[] { "6hD1gR", 3470067839 };
                yield return new object[] { "6hShWW", 3472660386 };
                yield return new object[] { "6hK4tb", 3471249260 };
                yield return new object[] { "6hLzrQ", 3471545214 };
                yield return new object[] { "6hBTAe", 3469850245 };
                yield return new object[] { "6hLABq", 3471549134 };
                yield return new object[] { "6hGbN7", 3470688570 };
                yield return new object[] { "6hFtro", 3470549444 };
                yield return new object[] { "6hRRAQ", 3472575120 };
                yield return new object[] { "6hFViL", 3470636466 };
                yield return new object[] { "6hFkLP", 3470523659 };
                yield return new object[] { "6hNAKc", 3471939809 };
                yield return new object[] { "6hLLNE", 3471583426 };
                yield return new object[] { "6hJstp", 3471131533 };
                yield return new object[] { "6hHxv3", 3470953336 };
                yield return new object[] { "6hLToQ", 3471605592 };
                yield return new object[] { "6hJ74F", 3471062877 };
                yield return new object[] { "6hGjCA", 3470714930 };
                yield return new object[] { "6hCQoD", 3470034593 };
                yield return new object[] { "6hCqxX", 3469954397 };
                yield return new object[] { "6hCbg8", 3469906325 };
                yield return new object[] { "6hJwGw", 3471145750 };
                yield return new object[] { "6hP2Tt", 3472024389 };
                yield return new object[] { "6hHDuy", 3470973492 };
                yield return new object[] { "6hGRpq", 3470818450 };
                yield return new object[] { "6hDx8F", 3470171649 };
                yield return new object[] { "6hLhxU", 3471488378 };
                yield return new object[] { "6hFrkd", 3470542358 };
                yield return new object[] { "6hPc3D", 3472055197 };
                yield return new object[] { "6hJV29", 3471220838 };
                yield return new object[] { "6hCc3c", 3469908939 };
                yield return new object[] { "6hLycA", 3471541024 };
                yield return new object[] { "6hLd75", 3471473424 };
                yield return new object[] { "6hKJ1m", 3471378900 };
                yield return new object[] { "6hHgEG", 3470900072 };
                yield return new object[] { "6hFNfm", 3470612720 };
                yield return new object[] { "6hFsaF", 3470545169 };
                yield return new object[] { "6hERqV", 3470428313 };
                yield return new object[] { "6hEnYK", 3470335967 };
                yield return new object[] { "6hDGeT", 3470202285 };
                yield return new object[] { "6hFDZo", 3470584940 };
                yield return new object[] { "6hMvPE", 3471728136 };
                yield return new object[] { "6hKTro", 3471410628 };
                yield return new object[] { "6hKfXG", 3471287918 };
                yield return new object[] { "6hKeuU", 3471283000 };
                yield return new object[] { "6hHFYj", 3470981830 };
                yield return new object[] { "6hHDzj", 3470973768 };
                yield return new object[] { "6hCozt", 3469947757 };
                yield return new object[] { "6hKB8D", 3471355775 };
                yield return new object[] { "6hCtrc", 3469964097 };
                yield return new object[] { "6hDXcx", 3470252609 };
                yield return new object[] { "6hCxSR", 3469979041 };
                yield return new object[] { "6hC1Vk", 3469874901 };
                yield return new object[] { "6hKmaS", 3471305444 };
                yield return new object[] { "6hK9fn", 3471265337 };
                yield return new object[] { "6hFDH6", 3470583995 };
                yield return new object[] { "6hEB7c", 3470380131 };
                yield return new object[] { "6hC1E2", 3469874013 };
                yield return new object[] { "6hBZnx", 3469869693 };
                yield return new object[] { "6hBXNz", 3469864417 };
                yield return new object[] { "6hQKjm", 3472358868 };
                yield return new object[] { "6hHn4j", 3470918204 };
                yield return new object[] { "6hHiQ2", 3470907341 };
                yield return new object[] { "6hHhHb", 3470903580 };
                yield return new object[] { "6hGnBc", 3470724941 };
                yield return new object[] { "6hG6Ht", 3470671481 };
                yield return new object[] { "6hDvh6", 3470165409 };
                yield return new object[] { "6hCGtp", 3470007957 };
                yield return new object[] { "6hCnzi", 3469944383 };
                yield return new object[] { "6hMxEY", 3471734360 };
                yield return new object[] { "6hG9sL", 3470680720 };
                yield return new object[] { "6hCarn", 3469903555 };
                yield return new object[] { "6hLsdE", 3471520902 };
                yield return new object[] { "6hKnDa", 3471310391 };
                yield return new object[] { "6hKn2L", 3471308338 };
                yield return new object[] { "6hGpfH", 3470730481 };
                yield return new object[] { "6hRkJS", 3472474666 };
                yield return new object[] { "6hFEV3", 3470588052 };
                yield return new object[] { "6hE7VV", 3470285343 };
                yield return new object[] { "6hSSAq", 3472773572 };
                yield return new object[] { "6hNTtQ", 3471996106 };
                yield return new object[] { "6hMAuK", 3471743859 };
                yield return new object[] { "6hJ95H", 3471069665 };
                yield return new object[] { "6hHZ39", 3471039240 };
                yield return new object[] { "6hByNi", 3469787029 };
                yield return new object[] { "6hLLnb", 3471581948 };
                yield return new object[] { "6hHYoQ", 3471037076 };
                yield return new object[] { "6hHCLm", 3470971044 };
                yield return new object[] { "6hFHkC", 3470596206 };
                yield return new object[] { "6hDKq4", 3470212967 };
                yield return new object[] { "6hRapC", 3472439910 };
                yield return new object[] { "6hKJBs", 3471380936 };
                yield return new object[] { "6hHids", 3470905278 };
                yield return new object[] { "6hEJ8R", 3470403775 };
                yield return new object[] { "6hMY3L", 3471816360 };
                yield return new object[] { "6hFRAC", 3470623988 };
                yield return new object[] { "6hEP9c", 3470420615 };
                yield return new object[] { "6hEqVR", 3470345891 };
                yield return new object[] { "6hHGBX", 3470984013 };
                yield return new object[] { "6hEzFB", 3470375341 };
                yield return new object[] { "6hDnRp", 3470140429 };
                yield return new object[] { "6hDdQH", 3470110113 };
                yield return new object[] { "6hCK7B", 3470016843 };
                yield return new object[] { "6hCxvH", 3469977815 };
                yield return new object[] { "6hC4v4", 3469883585 };
                yield return new object[] { "6hC15g", 3469872055 };
                yield return new object[] { "6hGHRA", 3470793056 };
                yield return new object[] { "6hGCGL", 3470775724 };
                yield return new object[] { "6hGbuW", 3470687574 };
                yield return new object[] { "6hT7FY", 3472820990 };
                yield return new object[] { "6hMFHs", 3471761416 };
                yield return new object[] { "6hJybH", 3471150749 };
                yield return new object[] { "6hGEFs", 3470782376 };
                yield return new object[] { "6hCBnX", 3469990821 };
                yield return new object[] { "6hNJZt", 3471967549 };
                yield return new object[] { "6hMxUV", 3471735169 };
                yield return new object[] { "6hLoGG", 3471509072 };
                yield return new object[] { "6hJdy5", 3471084708 };
                yield return new object[] { "6hGnwp", 3470724663 };
                yield return new object[] { "6hGkhZ", 3470717157 };
                yield return new object[] { "6hG7yd", 3470674308 };
                yield return new object[] { "6hDAqF", 3470182727 };
                yield return new object[] { "6hPQVJ", 3472182628 };
                yield return new object[] { "6hHyqy", 3470956440 };
                yield return new object[] { "6hFG6k", 3470592013 };
                yield return new object[] { "6hTavC", 3472830482 };
                yield return new object[] { "6hJjzU", 3471104998 };
                yield return new object[] { "6hFE7r", 3470585349 };
                yield return new object[] { "6hNQU7", 3471987422 };
                yield return new object[] { "6hJYSj", 3471233782 };
                yield return new object[] { "6hFVRB", 3470638313 };
                yield return new object[] { "6hEeQt", 3470308575 };
                yield return new object[] { "6hBmnK", 3469745237 };
                yield return new object[] { "6hP9VU", 3472048078 };
                yield return new object[] { "6hJeDp", 3471088381 };
                yield return new object[] { "6hHV4d", 3471025846 };
                yield return new object[] { "6hFXmS", 3470643374 };
                yield return new object[] { "6hBgEn", 3469729381 };
                yield return new object[] { "6hNDjB", 3471948475 };
                yield return new object[] { "6hKEkd", 3471366538 };
                yield return new object[] { "6hJDq6", 3471168345 };
                yield return new object[] { "6hHbCG", 3470883136 };
                yield return new object[] { "6hCgN2", 3469924937 };
                yield return new object[] { "6hQa3S", 3472243594 };
                yield return new object[] { "6hLphv", 3471511033 };
                yield return new object[] { "6hHoqd", 3470922780 };
                yield return new object[] { "6hGT2W", 3470823932 };
                yield return new object[] { "6hEd6V", 3470302743 };
                yield return new object[] { "6hNac3", 3471853844 };
                yield return new object[] { "6hHzYe", 3470961641 };
                yield return new object[] { "6hRDAC", 3472534740 };
                yield return new object[] { "6hJ2bu", 3471046452 };
                yield return new object[] { "6hGRPN", 3470819864 };
                yield return new object[] { "6hFS6P", 3470625681 };
                yield return new object[] { "6hG8Yn", 3470679073 };
                yield return new object[] { "6hFSGB", 3470627699 };
                yield return new object[] { "6hFQhL", 3470619588 };
                yield return new object[] { "6hF4VT", 3470470361 };
                yield return new object[] { "6hE7vD", 3470283935 };
                yield return new object[] { "6hBeKa", 3469722931 };
                yield return new object[] { "6hPLs1", 3472167564 };
                yield return new object[] { "6hHcKm", 3470886886 };
                yield return new object[] { "6hG9KW", 3470681716 };
                yield return new object[] { "6hE8E4", 3470287787 };
                yield return new object[] { "6hNp1U", 3471900352 };
                yield return new object[] { "6hJ29T", 3471046359 };
                yield return new object[] { "6hHPLb", 3471008038 };
                yield return new object[] { "6hGWGq", 3470836256 };
                yield return new object[] { "6hEipV", 3470320607 };
                yield return new object[] { "6hMB8U", 3471746014 };
                yield return new object[] { "6hKWyr", 3471421129 };
                yield return new object[] { "6hKLxb", 3471387416 };
                yield return new object[] { "6hJstE", 3471131548 };
                yield return new object[] { "6hHk3k", 3470911419 };
                yield return new object[] { "6hPSdE", 3472186974 };
                yield return new object[] { "6hPfEY", 3472067396 };
                yield return new object[] { "6hGVSS", 3470833498 };
                yield return new object[] { "6hFVX4", 3470638629 };
                yield return new object[] { "6hFQRa", 3470621467 };
                yield return new object[] { "6hCsKK", 3469961809 };
                yield return new object[] { "6hJbdY", 3471076872 };
                yield return new object[] { "6hE2ok", 3470266691 };
                yield return new object[] { "6hCrc8", 3469956553 };
                yield return new object[] { "6hRgwS", 3472460514 };
                yield return new object[] { "6hPhLY", 3472074472 };
                yield return new object[] { "6hLTK1", 3471606762 };
                yield return new object[] { "6hHh5u", 3470901452 };
                yield return new object[] { "6hDiBB", 3470126173 };
                yield return new object[] { "6hD678", 3470084095 };
                yield return new object[] { "6hKMXC", 3471392198 };
                yield return new object[] { "6hBohi", 3469751649 };
                yield return new object[] { "6hJXRV", 3471230395 };
                yield return new object[] { "6hFzad", 3470568690 };
                yield return new object[] { "6hCCbH", 3469993533 };
                yield return new object[] { "6hLpoA", 3471511386 };
                yield return new object[] { "6hKZQN", 3471432170 };
                yield return new object[] { "6hG3Ax", 3470660987 };
                yield return new object[] { "6hT7kf", 3472819788 };
                yield return new object[] { "6hKrzq", 3471323630 };
                yield return new object[] { "6hHMHM", 3471001171 };
                yield return new object[] { "6hHL9q", 3470995872 };
                yield return new object[] { "6hBUkR", 3469852775 };
                yield return new object[] { "6hRRLf", 3472575666 };
                yield return new object[] { "6hJFUg", 3471176707 };
                yield return new object[] { "6hBML2", 3469830629 };
                yield return new object[] { "6hS9eE", 3472631080 };
                yield return new object[] { "6hPD3o", 3472142646 };
                yield return new object[] { "6hKWhL", 3471420220 };
                yield return new object[] { "6hJMpr", 3471195219 };
                yield return new object[] { "6hHzVA", 3470961488 };
                yield return new object[] { "6hGvu9", 3470751444 };
                yield return new object[] { "6hGkTu", 3470719158 };
                yield return new object[] { "6hF7bv", 3470477937 };
                yield return new object[] { "6hDzQp", 3470180739 };
                yield return new object[] { "6hQ2Mo", 3472219148 };
                yield return new object[] { "6hM7Tn", 3471650979 };
                yield return new object[] { "6hJWDp", 3471226305 };
                yield return new object[] { "6hJpX5", 3471123046 };
                yield return new object[] { "6hNLTn", 3471973923 };
                yield return new object[] { "6hGuRu", 3470749318 };
                yield return new object[] { "6hG1CM", 3470654389 };
                yield return new object[] { "6hEMDx", 3470415589 };
                yield return new object[] { "6hCYpx", 3470061557 };
                yield return new object[] { "6hCSTr", 3470042991 };
                yield return new object[] { "6hBnJr", 3469749801 };
                yield return new object[] { "6hRZSh", 3472602928 };
                yield return new object[] { "6hRtVW", 3472502220 };
                yield return new object[] { "6hDzc2", 3470178571 };
                yield return new object[] { "6hCMan", 3470023731 };
                yield return new object[] { "6hRfUj", 3472458394 };
                yield return new object[] { "6hLdME", 3471475720 };
                yield return new object[] { "6hE69P", 3470279363 };
                yield return new object[] { "6hDgaa", 3470117911 };
                yield return new object[] { "6hDeKg", 3470113161 };
                yield return new object[] { "6hKSis", 3471406804 };
                yield return new object[] { "6hKwYj", 3471341778 };
                yield return new object[] { "6hJ4Ro", 3471055436 };
                yield return new object[] { "6hHTug", 3471020571 };
                yield return new object[] { "6hHSH2", 3471017947 };
                yield return new object[] { "6hHCE6", 3470970681 };
                yield return new object[] { "6hG5R5", 3470668558 };
                yield return new object[] { "6hFaBX", 3470489505 };
                yield return new object[] { "6hL13o", 3471432842 };
                yield return new object[] { "6hJtQr", 3471136117 };
                yield return new object[] { "6hHpg3", 3470925612 };
                yield return new object[] { "6hGinv", 3470710691 };
                yield return new object[] { "6hFQXR", 3470621855 };
                yield return new object[] { "6hCKN6", 3470019133 };
                yield return new object[] { "6hJme9", 3471110522 };
                yield return new object[] { "6hGUY8", 3470830439 };
                yield return new object[] { "6hEDPM", 3470389271 };
                yield return new object[] { "6hBg1c", 3469727167 };
                yield return new object[] { "6hNyoj", 3471931870 };
                yield return new object[] { "6hNdQu", 3471866108 };
                yield return new object[] { "6hMNAK", 3471784575 };
                yield return new object[] { "6hHkvV", 3470913019 };
                yield return new object[] { "6hDHLi", 3470207413 };
                yield return new object[] { "6hCwUk", 3469975763 };
                yield return new object[] { "6hCd2a", 3469912243 };
                yield return new object[] { "6hPeRd", 3472064626 };
                yield return new object[] { "6hP4SL", 3472031076 };
                yield return new object[] { "6hJQFC", 3471206250 };
                yield return new object[] { "6hJLVJ", 3471193612 };
                yield return new object[] { "6hJAJT", 3471159343 };
                yield return new object[] { "6hJ8Mi", 3471068655 };
                yield return new object[] { "6hJ6o5", 3471060580 };
                yield return new object[] { "6hH667", 3470864484 };
                yield return new object[] { "6hH5RU", 3470863718 };
                yield return new object[] { "6hC2jx", 3469876247 };
                yield return new object[] { "6hPm75", 3472085672 };
                yield return new object[] { "6hN8ud", 3471848112 };
                yield return new object[] { "6hLD4g", 3471557361 };
                yield return new object[] { "6hGAr2", 3470768083 };
                yield return new object[] { "6hFVQH", 3470638261 };
                yield return new object[] { "6hDxtV", 3470172823 };
                yield return new object[] { "6hPNwj", 3472174542 };
                yield return new object[] { "6hKTB7", 3471411192 };
                yield return new object[] { "6hJdfQ", 3471083708 };
                yield return new object[] { "6hRvdq", 3472506540 };
                yield return new object[] { "6hNpN9", 3471902976 };
                yield return new object[] { "6hMd75", 3471668536 };
                yield return new object[] { "6hLkks", 3471497748 };
                yield return new object[] { "6hHkYn", 3470914553 };
                yield return new object[] { "6hGZgY", 3470844930 };
                yield return new object[] { "6hGorv", 3470727743 };
                yield return new object[] { "6hG941", 3470679342 };
                yield return new object[] { "6hC6xK", 3469890469 };
                yield return new object[] { "6hTA5N", 3472913142 };
                yield return new object[] { "6hNzGE", 3471936298 };
                yield return new object[] { "6hN3F1", 3471831918 };
                yield return new object[] { "6hLdgN", 3471473988 };
                yield return new object[] { "6hFyvS", 3470566524 };
                yield return new object[] { "6hCxUM", 3469979153 };
                yield return new object[] { "6hCmje", 3469940145 };
                yield return new object[] { "6hC87z", 3469895737 };
                yield return new object[] { "6hC4mV", 3469883113 };
                yield return new object[] { "6hQXaJ", 3472398736 };
                yield return new object[] { "6hP6Tw", 3472037848 };
                yield return new object[] { "6hLx7r", 3471537361 };
                yield return new object[] { "6hFvYh", 3470557964 };
                yield return new object[] { "6hEPWM", 3470423317 };
                yield return new object[] { "6hDKDT", 3470213769 };
                yield return new object[] { "6hBrcn", 3469761455 };
                yield return new object[] { "6hBkh4", 3469741543 };
                yield return new object[] { "6hMdbq", 3471668788 };
                yield return new object[] { "6hK3cE", 3471244996 };
                yield return new object[] { "6hJSUj", 3471213714 };
                yield return new object[] { "6hJvFs", 3471142324 };
                yield return new object[] { "6hHVMu", 3471028298 };
                yield return new object[] { "6hHUZG", 3471025642 };
                yield return new object[] { "6hHeVT", 3470894225 };
                yield return new object[] { "6hFqjr", 3470538949 };
                yield return new object[] { "6hETNt", 3470436291 };
                yield return new object[] { "6hEPkX", 3470421297 };
                yield return new object[] { "6hCVrB", 3470051585 };
                yield return new object[] { "6hCE2V", 3469999751 };
                yield return new object[] { "6hNe3j", 3471866794 };
                yield return new object[] { "6hKjmA", 3471299338 };
                yield return new object[] { "6hHbMp", 3470883641 };
                yield return new object[] { "6hGWJ7", 3470836354 };
                yield return new object[] { "6hGn3F", 3470723055 };
                yield return new object[] { "6hFoKL", 3470533690 };
                yield return new object[] { "6hETWx", 3470436759 };
                yield return new object[] { "6hPaH5", 3472050698 };
                yield return new object[] { "6hNFFf", 3471956400 };
                yield return new object[] { "6hNnyp", 3471895451 };
                yield return new object[] { "6hM7ra", 3471649459 };
                yield return new object[] { "6hHof7", 3470922194 };
                yield return new object[] { "6hH8KM", 3470873455 };
                yield return new object[] { "6hGbCG", 3470688024 };
                yield return new object[] { "6hDXaB", 3470252497 };
                yield return new object[] { "6hDSF2", 3470237383 };
                yield return new object[] { "6hDfq8", 3470115415 };
                yield return new object[] { "6hCqqR", 3469953985 };
                yield return new object[] { "6hPpj7", 3472096462 };
                yield return new object[] { "6hMSHG", 3471798434 };
                yield return new object[] { "6hKyF6", 3471347507 };
                yield return new object[] { "6hK33v", 3471244465 };
                yield return new object[] { "6hJwva", 3471145091 };
                yield return new object[] { "6hGX9e", 3470837753 };
                yield return new object[] { "6hFWeR", 3470639603 };
                yield return new object[] { "6hD8oF", 3470091783 };
                yield return new object[] { "6hCopg", 3469947165 };
                yield return new object[] { "6hC9L8", 3469901279 };
                yield return new object[] { "6hBhS4", 3469733423 };
                yield return new object[] { "6hMQXy", 3471792510 };
                yield return new object[] { "6hLoRU", 3471509606 };
                yield return new object[] { "6hKCib", 3471359692 };
                yield return new object[] { "6hKder", 3471278739 };
                yield return new object[] { "6hHYPy", 3471038510 };
                yield return new object[] { "6hD4hz", 3470077973 };
                yield return new object[] { "6hPgTS", 3472071508 };
                yield return new object[] { "6hNXii", 3472008951 };
                yield return new object[] { "6hKYzE", 3471427928 };
                yield return new object[] { "6hKSr2", 3471407243 };
                yield return new object[] { "6hG4fB", 3470663195 };
                yield return new object[] { "6hFyz2", 3470566707 };
                yield return new object[] { "6hFoMT", 3470533813 };
                yield return new object[] { "6hC5V4", 3469888341 };
                yield return new object[] { "6hBZmZ", 3469869661 };
                yield return new object[] { "6hPXKU", 3472205606 };
                yield return new object[] { "6hHVwm", 3471027420 };
                yield return new object[] { "6hCzFH", 3469985123 };
                yield return new object[] { "6hCvsM", 3469970917 };
                yield return new object[] { "6hChKr", 3469928151 };
                yield return new object[] { "6hBv6F", 3469774581 };
                yield return new object[] { "6hPf62", 3472065427 };
                yield return new object[] { "6hNS4s", 3471991328 };
                yield return new object[] { "6hNPXe", 3471984239 };
                yield return new object[] { "6hLguh", 3471484804 };
                yield return new object[] { "6hJ5zB", 3471057885 };
                yield return new object[] { "6hHr6j", 3470931776 };
                yield return new object[] { "6hLBUx", 3471553491 };
                yield return new object[] { "6hLi1P", 3471489939 };
                yield return new object[] { "6hKQ35", 3471399184 };
                yield return new object[] { "6hKK67", 3471382540 };
                yield return new object[] { "6hHUcT", 3471022985 };
                yield return new object[] { "6hGPjm", 3470811428 };
                yield return new object[] { "6hF5ti", 3470472183 };
                yield return new object[] { "6hDZPz", 3470261427 };
                yield return new object[] { "6hC17D", 3469872193 };
                yield return new object[] { "6hR7R3", 3472431292 };
                yield return new object[] { "6hN7hS", 3471844090 };
                yield return new object[] { "6hHqoC", 3470929416 };
                yield return new object[] { "6hFDdx", 3470582339 };
                yield return new object[] { "6hFzdL", 3470568896 };
                yield return new object[] { "6hDuEH", 3470163357 };
                yield return new object[] { "6hCaZk", 3469905409 };
                yield return new object[] { "6hT2Hw", 3472804260 };
                yield return new object[] { "6hPhJY", 3472074356 };
                yield return new object[] { "6hKNBy", 3471394398 };
                yield return new object[] { "6hJPEq", 3471202816 };
                yield return new object[] { "6hGMH7", 3470806020 };
                yield return new object[] { "6hGp5L", 3470729904 };
                yield return new object[] { "6hFfRV", 3470507135 };
                yield return new object[] { "6hESHt", 3470432637 };

                #endregion
            }
        }

        [Fact]
        public static void RoundTripBase58()
        {
            // Arrange
            long value = 3471391110;

            // Act & Assert
            Assert.Equal(value, Int64Encoder.FromBase58String(Int64Encoder.ToBase58String(value)));
        }

        [Fact]
        public static void RoundTripFlickrBase58String()
        {
            // Arrange
            long value = 3471391110;

            // Act & Assert
            Assert.Equal(value, Int64Encoder.FromFlickrBase58String(Int64Encoder.ToFlickrBase58String(value)));
        }

        public static class TheFromBase58StringMethod
        {
            public static IEnumerable<object[]> SampleData { get { return Int64EncoderFacts.Base58SampleData; } }

            [Fact]
            public static void ThrowsArgumentNullException_ForNull()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => Int64Encoder.FromBase58String(null));
            }

            [Theory]
            [PropertyData("SampleData")]
            public static void Succeeds_ForSampleData(string value, long expectedValue)
            {
                // Act & Assert
                Assert.Equal(expectedValue, Int64Encoder.FromBase58String(value));
            }
        }

        public static class TheFromFlickrBase58StringMethod
        {
            public static IEnumerable<object[]> SampleData { get { return Int64EncoderFacts.FlickrBase58SampleData; } }

            [Fact]
            public static void ThrowsArgumentNullException_ForNull()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(delegate { Int64Encoder.FromFlickrBase58String(null); });
            }

            [Theory]
            [PropertyData("SampleData")]
            public static void Succeeds_ForSampleData(string value, long expectedValue)
            {
                // Act & Assert
                Assert.Equal(expectedValue, Int64Encoder.FromFlickrBase58String(value));
            }
        }

        public static class TheToBase58StringMethod
        {
            public static IEnumerable<object[]> SampleData { get { return Int64EncoderFacts.Base58SampleData; } }

            [Fact]
            public static void ThrowsArgumentOutOfRangeException_ForNegativeValue()
            {
                // Act & Assert
                Assert.Throws<ArgumentOutOfRangeException>(() => Int64Encoder.ToBase58String(-1));
            }

            [Theory]
            [PropertyData("SampleData")]
            public static void Succeeds_ForSampleData(string expectedValue, long value)
            {
                // Act & Assert
                Assert.Equal(expectedValue, Int64Encoder.ToBase58String(value));
            }
        }

        public static class TheToFlickrBase58StringMethod
        {
            public static IEnumerable<object[]> SampleData { get { return Int64EncoderFacts.FlickrBase58SampleData; } }

            [Fact]
            public static void ThrowsArgumentOutOfRangeException_ForNegativeValue()
            {
                // Act & Assert
                Assert.Throws<ArgumentOutOfRangeException>(delegate { Int64Encoder.ToFlickrBase58String(-1); });
            }

            [Theory]
            [PropertyData("SampleData")]
            public static void Succeeds_ForSampleData(string expectedValue, long value)
            {
                // Act & Assert
                Assert.Equal(expectedValue, Int64Encoder.ToFlickrBase58String(value));
            }
        }
    }
}
