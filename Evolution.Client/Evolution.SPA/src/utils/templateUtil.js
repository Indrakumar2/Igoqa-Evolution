import { numberFormat } from './commonUtils';
import moment from 'moment';

export const GetRateScheduleTemplate = (
    contractHolderCompany,
    customerName,
    contractNumber,
    customerContractNumber,
    cutOffdate,
    scheduleRates
) => {
    let body = [];
    const documentDefinition =  {
            content:[
            ],
            pageSize: 'A4',
            header: function(page, pages) { 
                return { 
                    table:{
                        widths: [ '50%', '50%' ],
                        
                        body:[
                            [
                                {
                                    table:{
                                        body:[
                                            // Intertek logo
                                            [ {
                                                style:[ 'm_l_50','mt_20' ],
                                                width: 100,
                                                image: 'data:image/jpeg;base64,/9j/4AAQSkZJRgABAQIAOwA7AAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCABLAN0DAREAAhEBAxEB/8QAHwAAAgICAwEBAQAAAAAAAAAAAAoJCwcIBAUGAgMB/8QAOhAAAQQDAAEDAwIDBQUJAAAABAIDBQYBBwgJAAoSERMUFSEWIjEXGDl4tyMyOEF2JDQ2Q3F0obW2/8QAGwEBAAMBAQEBAAAAAAAAAAAAAAEDBAIFCAr/xABBEQABAwIEAwMICAQFBQAAAAABAAIRAyESMUFRBGFxgZGxBRMiMqHB0fAGFDNCUnKCsgdzkuEIFWKzwhcjJDTx/9oADAMBAAIRAxEAPwBr2wQm8Fb7IkBhrEqb/iEhuJk22zf0tuDUa4oRDZKU/ior+Aft4ebUr8bDHzYPTkr8prP5wfp19E/8ZdX/ABvcd5V4DyZ/EWp9N/8AqPXH0V+kfCcL5ZP0RofQF/lt7vJ1Ph/KuA+ROH+gzPo8WU/KnAcRXbQ819b4fyjQf5RfWa/7f8k+U/4YD+E9LhK9fyOPJn+UUn+UOCe/hvr7vKreGY2u51En6w7yseJxGnUa3zxqxU4QigKLxJ36/R8viBHoiPREeiLDvRFwnNe8/wC9L/WX2hrJR9O7NuFfJfZQSwPOVmlTc1EvvDuf7N9po8Idxxlf8jqE5Qr9lZ9DYE7KWiXNByJA7yqqvwldh9Ng+ZLmq9P7q2FNWfpTfFeo++pKwWiYmF7art9klx80JemzS3WrCoZwxMrBLkEPfoMwDGnReBXAmcJL06zGebqDCIa0lsaEEAR2WO4srbb0Xlo9ER6Ij0RHoiPREeiI9ER6Iq2v3fvQm5TPIHrHQ2NhWYLUWu9C6+2NV6JGSxsdAC7Bs1pviJS7kgiPNMm2hIkLEx0dLkpcKiQQcMRixPyjlFFv4VoDC6BiLiJ1i1umvPXSHC/AVvDaXQ/iY5I2dua3y9+2EZXbtWpa42Et6RsM8DR9m3KnV4qekyVuFS0uzXoSLDOljXXj5R0XJ8iQSeQSS7A959hhZa4AqvAECQYGQkAmO0qYn1KqR6Ij0RHoiPRFp35C9n3PSvCHYu3NdSqoK/a35p3RcqZONttuuwtngKBOyEJLsNupWysiMkGRzR0vIcay+w391taPkjJd0wHVGNOTntB6FwBVbN7bvqXoGF8yGjohO2bzKRnTU7siH3yFP2aYnGNo/DWF9uAcxcMSZhOZuyRlmhwZeNsR+X5gR/BbTRmBZGQYKLdxDW+ad6I9GMMWw3AtyvkrV30XnI9ER6IkMdqe7a6ApXkSs2qYPQWqj+SKVvSV09IRhjFpRuqwVuAu79PlL0Jak2ZFai7AY2KRORVZdqJMaO3kaDPkH3/vzeY56LaOFaWSXHEWYtIBw4o5iLE75J83CsKThWP6ZxhWM/0/bOPrj/49SsSVZ8kfuq+SePbrZtMc3UkvrvbFUOMhbNPw1oFqmkqxOhOJYMikXlmPsEheJGOfw6we1U4fMAh9pwZu2qMZJGZLTT4ZzhLjgB0gl3dYAHmZ1hQfNe877WxM4fe5P5ccr/3vrmLaK2wzL5H+f1+3idXd3g8P/b/k/IzXlN/L+f8AF+n8norfqrPxO9nwU+HjL90PyD3Jda3pLdtUP5K3naygoips2eyB2fUd5sBriRxIGC2H+BAkQNgkiVIZi4i3QMYFIEvDRkbYZKXKGAeKmpwzmSW+m0XMCHAbkXsNSDzyup7uv/8AhL6i/wAuu7P9NLN6g5HofBUs9dv5m+IVNTxD0aJyH15zt08fVCL0FovbNS2UVThJduAKsjNYk2j1xLE09HSzUY6Zhv7aDHIw5DOc/PIzuMfHMr1qgxCo2YxAic4vtbbdOux3vV9Vunhty3AF/Ci1ksokDI7oKvSh4oanE4IfDjSdUQ45xLTXyWyI/KxzT7mEtrNHSrLqYvsO/wDsPFY/qm1S/wCWP+R8E4/zzvfXvT+jNUdDaoOMkdc7kote2BUCJINUdKJh7EA0cyHLR6lu5Al45bjkfKhYeeSLIiksIfeQ2l1crI5pa4tOYJB7FHv5LfNLxP4vItiN3ZbZC57lmI1MpWNA6zbAm9jyAD+HcBzM+ks0GGo9bJdaUhiXtEgE9JIQQqvxs44ISy2VlOi+pdohuRcbCc45nkEqRsT3pO/C7AUrU/FGoK/VUkuYCY2HsS53CwPiYX/slkl1sOjxwpC2/wB1ttBFttrz9Euu4x9cloHCt1eT0AHjPztpunx37x/Rl/ssVUOz+dJ3RAsmW0Hna2rbAVs2mReXsIT+bZKabERNziYtpz5Zffr5F3PQjKcpi14StWC5dwpF2OxciIJ5CLT1hOK6x2hrrdNAqm1NS3WtbF1xeIgaeqN1qEsJOV6fiSsZ+2XHyILjrDuEOIcHJZypJARjJARjLBY7zLZZSC0kEEEZg2IXu/RQoZvIv53uAfGxIl0bad6ldmbxHFwQrRmmxI+03SJ+62lwXN0kC5KLqtFwQhbTzYVinBrA+G62cDAmiLQ6oradF9S4gDcmO7U/GygdrHvStTyWzoKKtPD12rOozZcUKeuIO5ImyXaEiXyUMvzo9LTr6Hi5Z0JhSi3IRu2COP4bUOxJqcyhSiv+qGLPBOgwxJ2mbdeuVphn92fYIe2eUCmWmvGtScBZeOtFWCDkmcLwzIQ8zN7JkYw1rDiUOYaLCJYfbwtCV4Q5jCkpV9cYKzhvs/1H3LYbxge6Fpnjx4j03yLK8cWbaZ+q8XXD95j92RlREmv4tvtmujeWoAnWVheB/BbsCI9fylifvrFyTjDWHfsoi4yvnyzM7FRU4fzj3OxxMWwzkAM5GybH8PHnJ0z5dV7ZrNY1PbNHbS1EFC2CZpNiscbdI2bp08WRGC2Gv2mOia+p9wCVHSDORh8BHuArkIl0QmTaLfyHKzVaJpQZxNMiYi40iTpF+uylw3bvPUHN2srTuXe2xKvq3V9LC/Pslyt0i3HRMe0pWGxx2/rhZMhJnkKbDioaMHMl5c91gCLBLNfZYWVQaXGGgknQJN3rD3l+sKvY5Gt8acuS+1IgEh0dnZ26rM/QIiXw3j4pNhtfQEfLWJyNeXj5juz1hrUqtrOPyoQF3GW/Ram8KSJe6DsBPSTl1jlzjWPVvvSt0C2EPG6+J9XztUcISk9zVuxbZU7CEIr6/N4Nq2A3WNkyG/2ykV56Jaf/AHTksf6/PBdHhR915B5gHwiPam2/HF5cuMfKBUjZXnW8lAbBroAx160hfxxK7tanMv8A2m1HvQrJ0gBY64kp1AmLTUpObhGyHRxJAmOkX0AYLNUpPp+sLHIi4PzzXtfLD/hj9+/5Q9/f6a2L0Sj9rS/mM/cFWUe3U/xoeF/+tdj/AOhm0fRbuI+xf+n9zVbpGmhxoZcjIljAR4Az5pxxr7QoYQYrS3yiyyn1tsDDDMNreffeWhplpC3HFpQnOcF5qTJ8jXu5te6E3Q9qPhnVNM6Ug6ce6Dfty3GxTcXQZ2TGI+0ZCasFrqGjZ6OEwh1tV/NOxCnl/XMDCzUOkabkC1s4UlsvJaTcNi4G7tumcZkZJoHgfrmv94cfaI61rNYkKXGbnqDs49UZQpEgVW5yHnJaqWeGTJIHDTKhAWWAlh4yWwGFmUjUCHrCCWQoVosz24HuYb4SRO/ONOmip8N7f8ee5P8ANvsP/WKX9Qcj0PgvVHqD+UP9tWSXudu7Ljxl44Cqtq2cNre0eqLazpCKsUUU4FM1uivwMjO7OmogthxskQ8uBCGprRoykFR2LiqRDfHOEFdTKwcOwPqSRLWjEconIA8p2ukV/CX4g7V5bOhLLUZC1yOttC6fh4qybp2HEgjnzyETxZYlXo9OYOSuOxbLYuNmH2JGTaJjIKIhZaVKDkyWo6GlS11aopNmJc6Q0H2k6wJFtSc808yX7UXw+kUtdXY1vuYGdUDkVGx2N42924oJ+38MSf4BeSNfqL+X0dyzmj4jsr+uEgpRnKfRY/rNXcdI9m/tSGPmN8Vd88THUQupjrQRsDVV/gnL5o7ZzgTcVIz1aYknY6Sg7GEM44KDdabIoYDnEgOfhHCHQlgDaAZm0RYBbaVXzjcQsRAcOfLkf7FPWeIruq291+B7cc/s6bIse3dE6b6E5/2FYj31kyloVTtRFy9Ns8s+7lbxUtJ0SxV0WakCXHCpaejZaVIXlw3PqDkeh8FjqMDK4AyLmuAAgCTcDkDICroOBeeKv1p2pzLzRdZuerdT3huOm64sE9V8x+LFExdllWgCjobMsHIRuD2G3MqHyaCWPhf0y4O6nHwzK9CocLajvwgkdZ1T5MZ7M/gcWSAJkulutJWPHMGfOjMSOogf1ARp1C3wvzWdYreEwU2lTOSGU5dawvK2/ovCc4LD9af+Fvt+KmR8gPTmpfCv4wZe16uq0XHRmmqLVdH8z66LIIKDkrybH5r9BjZJ8h/9Qlg4YcM66W950v8AU5aIgJ13Jf6iUl7L5+d1UxprVIJkuJc48s3HRVgHMHNHYPmU7dLptempDY28NwzcvsPbW2b4cU5D1SAQSP8AxJf7rJMMPqj4CGSUDEw0NFif7Qh6BqFYjEZfjQUF6DnMpMnJosGjMnQDmbknLMp67RPtA/HJSKMBHbyvG+N47DdEb/XbSBbgtZVhEhlpGH81ipQMWefHhIe+eWG560WV9Scpy47j90eixu4qoTYNaNonvJ+AUM/mB9q6fyxqW49O8H3a77a13r2NOs2y9JX5uNlNlVmmxrCy5i3UmyQEbDiXSMrobb0hOV42Cj58KGFKkwJCwOMuANlbS4nEcLwATkRkTlBvbrvmtZ/bD+Vq5ck9Z1PjfZVpLM5h6ltQtUiomWLU4BrDeM8pqPpVrr/5Dn24wC8S2AKTcAWPsimuycDYilYdrq8FF1xFLE0uFnMvldw1HZmLbpu33EXlYmPGfyCDF6hlGAen+jzpqjaiPykYl6gwUUEM7ftrJCJ+bRBtXFlYiIq7ZDLo2LXY4qTIHOAhpEF8stCmKj/S9Vok5Sdh2nkbAquq8evjg608uPQ89StTOOSRbZX8Zbv3rsiSljK5SxbFJEukWK4zy0yEzYbXZj0yLkPCDfm2CzyDMgW4piOBmpiOLfUe2m3E4xo1ozMaAaADXISBqnT+f/Zw8Y0OXpNi3x0TuzeBUAYDJ2iowkZVdZUG3PC/B5yGLYZHtdxDgCH05aMxH28KVKD+SGD451z7iCyHin3wta3Y3JHgJ7OxQB+7ejY+G8p9ah4kMeOi4rkbScbGx4bSGBAY8Gw7MFDDFYbxhDI4o7TbDDSMYQ20hKE4xjGMeiu4b7P9R9y3b8TPtlOTPIHwPo3rLZG/eiaZdNopvmZqt0hetW6vGqqmxrXTQkxeJ6kTUsrBEfACllLLkHc5MIIS0hphLafQX9vsMLmpxDmPc0NaQIuZ1APvTWXiw8LnK3iaD2WZo+a2NsC+7ZRDgW3Ym0pODNm0VyvvmFxNXg4+tQVdhYiJQce/ISC0AvyUsYgNRxzg8dHDClmqVnVYmAG5Ac8ykF/cXeVe49+djXDTlIspjfKfM1vm6FreuRxjuIa93ivkEQN029LMtfBmVLlpVmRh6S+9h9mMpI4r8ckQyx2FRxbaFPzbP9TwC46gHJvvO56KVvxVe0wD3Bqyob88iN4v1BavUSBZapzrrZyNr9wjYCTGyVFl7Tt03FTa4SVkRHhjHKTBQ7MtDMraZmbCFMZOg40qqnEw4tpgEC2I3BOuECLbE55xdSTdJ+z74GvdFkWeadkbn0Ns0YIhdfk7PYhdp0I+RSwvIo9or8nGxtjSGQR9tDxsDaQXQkKUQiOkct4DeKtvFPHrAOHcfh7Ei9dad2d4a+6/0g0yR1B0tztbAJiEnoMl4yuWeHJQkmNmYkpbYo1x1pf4F1TJQRoyB5aHNkIGfjgpFmTjBC2jBUZPrMcNRlv0cDkRyVlHsrtGpeQX2/nRvV1TEYiM7K4b39/F1YYfUQmn7Er1BtVevtWS67nL7gsTZ42RREkk4Q+fBuRkktCPzMY9Fgaw0+IY06VGQdwXAg9oVeZ4Hdla/wBOeWLkbae1blXde65oUxtWy3G6WyUFhq9XYSO0RtFwk+SkTFtsso+uUMDtYyp8wx4cIRp8shhlwtlcF1JwAknDA/UFJR5yvcZbD73fs3MnJh1i1Zxy0URGWWe+pMJsDopkd7KPvWVKFNm1jWJGW8PR1Cwts6dYUg285WtxisQRcUaAZ6ToL9sw3pu7nppusMeEn2/u4vJfZIfdG52rDp7imEksLkbp+N+Dcd0kAEfEypakYPYW2mN+604FYdjFDkQsKv7wEMPPzo5ocWU1qwpiBDnnSbN/N8O+FaD6c1BrfQGrKFpXUFUjqPrHWVZjKhSapFfeyHDQUSwlgUfDxLr5hhLmcLJPkTyCZCTPfJkJAok0l99wvPJLiXEySSSdyVS+b2/489yf5t9h/wCsUv6g5HofBesPUH8of7adG96iteNS+PxvC1YbXe9/LUjGf5VLbreqEtrzj+mVIS65hOf+WHF4x/vZ9SsnCZ1fyt/cvZ+y1HYTzb20WllpJT27tajvEYRjDzrAtElXBmXHP95TTDhZS2kZz9ELJeUn6ZcV9ScXnT/Kf3FOqeixpFn3rYYv8L+PY/8AHa/NxPdHB4K+GPv4FVH6ceUP9z/e+1l1KXMo+v0+eMK/r6jUdD7lr4TN40hvsn4lcb2vLZBXiI8qojCHX3XpPaLY4zeFLU4QRzLhpKGm8f1deVhpvGE4+S84Qn9/pj6DkehXXE/a0eg/eUq14ZTBQfK94+yDCGRWP71moWcvPrS23h0q1gijN5WrOE4U8Q80y3jOcfVxxKf+fqVprfZ1vyu8QrlT0XkpJz3p1onQ9BcNUwZ55NZsG3du2aYYTlWGHZuo06pxldcd+mPhlxkG6WbDOFZ+vxdeynGforOC2cIL1Ds1o73f2XB9ltrulMaT7X2y2MG9sWT2lrrXZZiktrkI+lwlTOskeMyrP1dHDmZywSbpSU/Fs1+CEyv5qAb+BOLmaY0wk9uIg+wBZA9y15kPIJ4+egNIaT5VLj9RUG3aoVsSR2yfQq3dDb7Z3LTNwUlToki6Q87X4wOlR8XCyEoMABiceftgTxpTMcuPaJLnh6THgufeHRhmLRINr78rJZd/3M/mbKZeGK6niyhiGnGCRiNF6FeHIYeRlt5h9lzWqm3WXm1KbdaWnKHEKUlWMpznHotPmKH4D/W5Q2ahB2Zc9466G1RASs9tmd2XWyaFX6bFrzKnXV6xCnQgleioplP2HEymGchsBstshttpyjDTLP1QVhIAMwBBmcoIgybnLqe1Mre712DbrH5GtQUexO/bCoHI+uHxYtpz5hiT1xt+wZW0FMJTnLX3CnxI8NbycfJ4eLESpSm2WcJKjhRFIn8TzP6QI8Smlvas6Pp2rfEhq+/QccI3bN/3/aWxb3MoZRg2SLg7tM64rgLxOU/eUFD1ymhYEEyvLA5p0sUwhDkgRlYfPZb3LNxJJqkaNDQOVgT7SUyB6KhVhXu9GHmvK5BuuNLQ0TyhqBwdxSc4Q8hFt2mwtTav6Kwh5pxtX0/opOcZ/wCX1L0OG+z/AFH3Jwr2y5whvhf5MwIQ0RkMndIJWGl4VkctneWxFOju4x+6HUJcbUpGf3+K0q/orGcwPefErLxH2zv0/tCmF6NsczT+et8W2uZcTYatpnaFjglNfLLqZmEpE5JxeW8I/n+5g0VjKPh/N8vp8f3+nqVU0SQDkSBtqqfXxLUCobh8n3DtG2U0PK1S0dOaxVYQZX6Piz/4lnFmWoaRS9lWC2LBIhDxRbLvywWg5bK8Lw7lOS9Wr6LKpFoa6PD3q158ofSG3eROA+nujtD0ti+bX1XrtydqUIZHlS8eE+TMxMPI22ViQnGSZSJocPJSF2lAEPMtlAV8hop9gRT7zZebSaH1GNdkTfTs7clWzPe5y80Trrrv96qNa+64tz7TOjdEpab+asq+20nOuFZS2j6/FCcqVnCcYxnOf6+i3+YofgP9blGv2t5AOp/IXd6jsfrC9xmx7vSKs5Sq/YhKJRqWe1V1Sxs41EHKpNer7UoOHKSUkWDmRbJdCXImpHcbbIWjJdtYxgIYIBMkSTfLVN4eFmH2rEe2m8qC7vBTkTQ5qvdTzuozZgV8QeehCOeYKOs0lXsEJRkuCRZ4owNJ42FBPTA8wO04p8QrCSzVI+s0bycVOcrekI9m94hIf/8Ap6LX0Wa+cL1q3WO+NT7B3dqUfe2pahd4Od2BqAufkKuxf6yCUlyQgFzkXnBYX30fR1Cc4WIWtlIUi09HklNLKHAwQDhJBAMZEjb5jMK5n4c6e5l665m1nuLkeXr5WljoIKCgq1AxsfXndbEwYQopOtJ6oR3xHps5T28jx5FeabSEyJ+EdDOnQJ0VImF5T2ua4h2fjznWVtt6LlUl/RTiITu/ersxnMaiI612cuUyYlTOY9IG4ZvJuSkqx82si4ad++lSfk3lteFY+uM49Qcj0K9dvqN50wO0048U4x7zLYVCtur/AB2i1W61SykyMru+5AMQE/GS7plQmYDVDUPaB2wCX1OV+Xdaebi5bGPwpFbBCQ3nsjEYalZeEBBqyIsB2h1x1WcPZcDvp5d7RLUy4kZ/fVDHZfynOGnHxdfOOkMoX/RTjDZgq3E4z9UJIazn9l4+pRxedP8AKf3FOkeixpGL3rX/AIM8e/8A1P0Z/wDVae9RqOh8QtfCZv6DxKzL7NKOCmOJ+yYmSHbMjpToqPjpAR7HyaKCN1PXxih3U/t9W32HXGl4+v7pVnHqU4v1qf5P+TkmJ3zy7tjxd+QfZGpWnJasT2mNqCbD0ZdG21NOy1IRP4teoL9Ckrb+wQ6kEaOwYpj7zMdZoqYh3lZKjCW0FrY8VGB8TiEOHPJw7cxsCCrKPxMedfk/yQapp8XYb/TdPdZR8KEFsrR9wnAK4TMWINlgeSsupyJgkdi6VGbJzmRDjowguyVlt/MZPgJSONLShefVoupkmCWaO9t9iNdNliH3OPD1k7Q8a09adZRL1k2Zy3amN9wERFMqNkrFSY2GlITaELFNDtvPFEsVWRVcWQxkrfkX6azHCtullDoyXXDvDKkHJ4wzsZBB7xHaknvb8+XeM8WPTNlG22xLSHL/AEDHwVa27+jDESUvRpqulnO0zaEVDsZy9Lt13EzORdkhg28yMhXZoouOaPl4aKizS1V6ZqMt6zJIB13by3HO2qs6adsnjXt/XUFbqlZuf+nNbEpbmoctSqRsqHAfdZx9XX4uVaknK/MMNKyyaHIgx8uCvCxTRx3ULaSz5rzyHsMHE09o/wDq7UbknkIxhsoPmXm4sZ5PyZIG0xrF9h1P1yn5NutVtTa0/XGcfVKs4+uM4/rj1EDYdwTG/wDE7+o/FezpvPmhNczKLFr3SGoaJYG2XR252m61plYmW2H0ZbfYRJwkKCall5GcodbS/hDiM5StOcZzj1MAZCFBc42LiRzJKrX/AHdv+LFH/wCVnTv/AOh2R6L0OG+xH53eDU5N7an/AAXuO/8A2u4P9ddleoGXafErJxH2z/0/tap2PUqlI7e8a4atFtqOgu+6PCkSsdrEEnRe7nAh8uuwlYnpx6f1fajft/zNw41plbRV5M13GcNSVnqgv1wkjOUFs4V93Uz96HN5kWI7RFtYUeHtpfONrPhhFl4x64n3atz9sG4LumsdrEMkmxOp77NDgxthhrg2M2QWFQbakCOkETYrDg1TsLB50sx+k2CSloMu+Iomp6bbuaILdSBqL5jbbLKFYrVq5av3ZSsylJuNL2dQrdDEMImadZIa112chZUVYz/40rBmnAlilCvrby4yQpKkufTCvrn0WEgg3BB52Kp1u3eb9xeKTyKXjW4T0lWbZoTb0ZsrRVzyyvCZunBWJu36fvkY66jDByH48WNxJNtKeYEn4+agilZJji2kF6jHNqsBIkOEOB3iHDvuDsQbFWX3i083nH/kr1FV0v36kar6UbhRo/aXPV1no+El0WFoVpiYkdeYnnx0X+iyr6nC4omIcPlI0F9uPtAEdINrS8WCpRdTJsS3RwFo57HrHJSNY5X4+kDHW084c2GyDqFHPoxqDV5Jjjbzn8xjqcV5by0Our/mIXjKVuL/AHXlSv3iBsO5V43fid3n4rlY5B5MxnGccvc7YzjOM4zjSetcZxnGfrjOM4rP1xnGf3xnH74z+/pA2HcmN/4nf1H4rXfynxsdDeLfvGKiAAoqLjuOt8hx8bGisAgAiD6ysDbAoYYrbQ4ozLaUoaYYbQ02jGEoThOMY9SuqX2tL+Yz9wVYz7fesVu4eYvh2AttfhrPBv7AuZ78NYI0SXi3jYPUOxJ2FLeAOafFdIipqNj5aPdcaUoWRBFLa+LzDasFvrkik+DGWXNwnvy6WTW/uE/btAb2BtfbvBdGEjd2xwr83urQVTjmAgNwBCtZdNvGu4cJtocXaIzCFPTlcDaQzsJlCjY5pN2S6zbo+f7j3j3556FeIY8+j91x+7Oh/wBP7c8pSi/ik8rfQfia6DzdKYmRs2qLLICQ2+9BzJhMfD3eHjyFjuFiNPoWms7HrCVlKrVlSJl8N/JEPMDnwR8lGvytNWkKjYNiLtdtPiD/AHCtjOQOv9Dd0aFpnRvOdzFuOvLkL8VJz9sawVOwjNsqmqVdYVLzz9et1effQxKRZC1oW24LJxpMhCyMZJml5rmlji12Y7jzHIpGr3CXt7OkXOktmducTa5m92a33dOyF+2zqmjifqmyNc7Jl8rNuM/B1QdP6pcabcpb8mzq/h5qTnYGdlZcMuJbgmo4/wBFso124Qx5gtEAnKBkOUDv6pcXRXif8lnT2wIjXOv+QOg1yqnxIh+b2Fr22a7pVTB/Iwyp+w3K/R0HXoGNjvuuPujuHfl5RhxsAAstxsd1EZLQ6owCS9ttAQSegEzpf3C1pT4g/G7XPFzxhTudBJoK3bCk5aR2Nuq8R7Dg8daNoWQWOFk8QiCGWDE1mtxETDVSuqMZYLNjoVEwaIHIShgzZebVqeceXRAyAtYDKYAk6lSheirSfPu7+Puj+j9F8nbH0Nqa57diNJ3baQux4nXsBJ221QIOw4qjt1+f/hmDGNmTIBsupSAMvJBhvtRJBcWo3DY5mX2o1Hd4fPzbTwzmtc4OMSBE5GDvpy3Wafac8l9Bcw8O7klt/wCsbVqUzc+9MXGjVe9Q51bt5VRh6PXa/iwyVblWRpeFClJZiQZiWpUQMw0UBUm0PmNMjyiZTinNc9oaQcLYJFxOImJ1ibqR/wAuvh05/wDLHqQCDupOdb751+HIf2N72hoxmQla2o1X5BFUtsZl0RVu15KmJSQZBLOCOiT8ql67JRxT8mzKlXSqupG12nNvvGx+TKrousfb6+VXkqzywB/Ml03XTwH3VxOzudY8/bFbmAW3loYPVE10Vy81xa0pbUsW0VWGebccShnJKPi8stza9J18QbydY/A9iZy9pXz35D9SWXp2b6Npm8dZcyztNrsVU6XvCLtlXzObdHsKXFWClU27NCSgYkVUW5mLtM2HFhR0w9K1wJZUm/D/AG4t8/Pz4KjiXU3BuEgvkklsEBp0MWmbgdd10HmJ9qkdtG72vpLxquVWvzdpMkLDeeWrEcLVq4TOmOqKOlNMWYn7cHXUShLjr71BszkVXQCHHnK/Y4qO/DrYxKXEwA2pJiwcLmP9WpgWkXiLJQm9eMXydaMsRNds/FfWlcl85WK49WtRbCsUWchK8tZSHZaPFTVflxlr+SWnAJUsZ/8AfLS1p/f0WoVGOyqMPVwGUfij5FxZPV+095s7z575+6KX1bWNmaz1HdbfRz9D6v22LMwtoj5YCOsCNj26MqFhwzM1SvWJoumgt4MBARPyEISeMNlsVRhj58fnLv0xcS5rnNLSHHDcjraTqc57L7Np+izKu591/wAH9d7I751/0Dqvn/ae29V3PRND1+DYtWUey7A/SrtVLFdnJKt2EGqxkqdCGEBzMUfEPHjsiTDJTrMc+QUAewOW7hntFMtJAIcTBMSCBfbMbym3PBhzvtnlfxX8m6W3jVy6RtCArFunLNTpHGEy9ZzeNj3C7RMPOMYznIU6LB2CNxMxjn0IipJRMYUlBQjycBb2+0ys1ZwdVeWmRaDvAA9ylq9FUvF7G11Rdu0K36u2dVYW869v9elKpcqhYwmpGEsVdmhHApOLkRHcZS6OSM6tHyTlDzK/g+w40+224gpBIIIMEZFV3vk99px0dqG2WLZnjuTnoDS0gUZKMaamJ2PjN167Yefw7iFiip0iPhtnwILbim445iTCu2R2mRDIGbKafnDC20+JaQA+ztwLG+fLnooZuXPHn5h6L09q2u6W5n7C01tuN2HXS4S2ka72ZrysVSTEkxVLsNkuh0XG1MGux7CFPTZMnJrjSohskV9BbD+RniuNSkWmX0yMJJEgk2yAzJ22NxurIXy0+HDRnlg0zCQWwj29e9E68iCG9U78gIlow2CMKbS/I1i0Q6nhHLVreYkk5MfgXDw5CFMXmVr0kAS9KDyxYKVU0zIuDmN+Y5quv6y9vn5VeSLJLCH8zXLdlNj3nnIvZ3OYJ+2a7LAtPOIZOVDV0Ry+V1xSUNrUNZ6nDuoccQhhROPi6otza1N+Tg3k6x6bHsWWvEHw35Y3PIXy/dddaU6g1KPSNuUaZ2Js3YdK2Lrukwmq4ufDI2JF2WauMZFRkvGTNQZmYRVQQswqwunNxIoSniUuNEqvp4HBxYZBgDCTJyiJyMajuVsB6LzFqR31qW4764g640prwUc++7U5z3DQ6ZHlEtBjyFos1EnIqCj3TH1IYESfJEjCfkkLQOxl7Dr60NIWrBd03Br2OOTXtcegIKruPbyeOHt6t+XLRl/2BzPuTVtJ5xltjTu1rVs3Xtpo8FCLf1veKVHQgcnYouPDmp6WsM6CPHxsO8c+8FgqXxj9KEfLQW2u9hpEBzSXYYAIJzBmNBAzOtlZ/ei89Jpe4U9u830im1du8JUtljoNCSJvdejK6K0MPu9tOMum3iix7KW2GNtNpwp6chGcNtbGbws0RKbul1u3FqoV8PoP9X7p1by5tPszFpCXn8VXIPnrp8Dupziuh760NX5Ccpwt8Yu4M1qAOyT0SNZkR/6ODfGYHEwdBDGFsTj8Ww8sFMhEjyTiVOBtpK+q6icOMtOcQMRGUzDhGVp5q1F9F5yPREeiI9ER6Ij0RHoiPREeiI9ER6Ij0RHoiPREeiI9ER6Ij0RHoiPREeiI9ER6Ij0RHoiPREeiI9ES2Nh843bdW6JpPKE54WNxg9AbJpNg2TRdcr640U8fYaLVyDhZywszAcEVVxGQHo4xDghs6wa6pr/srBCHGVuFeKTCC7zwwgwTgdmeWZ7lNlROu9C2m1VjUNg21p2mdLytWhZu0czk7j11ObcpM4dCjSs3UpGuRE65JSMlWnnCgzSQI/LBTQS5QZH6e627kqi0wTBLR96DGcAzzWv2mvLNwPvnfO+OeNd9D68OunPcVFTlrmzrlSwaJaYYqsSlvsUtrS2Ksrgd6idbwkQa/s6Vimv0ykqRnEsWhtoh1gujTeGhxaYdYb7XGk6LK39+/mG4am3nszQG8tJ9GF6K1neNj2Oral27SLgehmnVuTnmwZNysSc89BMS7sdiNHlSwnBUPPfJCCFNqZy6KMDwWhzXNxEAYmkZ9Rdec5U7jqW/vH/rrvy/wbOnKRZ9KzO7bnDEzS7OPRq3WR5s2wLVMtRUS9LshR8EWa2tuIFIeQpDCRcv/RKiPYWvLAcRBABiJJiLdqioqvuD5FyO1Nvjbfju6K0twBvPYULr/W3Z1kuVElwVZtUsZDVO3XbUkU3/ABNTqZNFCLexOOzcmNkHH5EMqfceEGIK00BBAqNL2iSyDpnB1OkQDvEFT4r3fphr+1D7m29Zt/2IstE7m+d6rCf7JR34l2fZf2V9ZTH8DsvQbD8w07Zv0xDkYw8chWRmnHElRBtY+l6tje8W3va2qxZd+4OMNZv0oXYvWnNlFI2RARFr1+zbt361rzl0qtgytMDaKwmVsouZutzSm3MRU9H/AH4mQy07gUx3LTnxKQx5khriBIJDTYiJBtpInqN1qzvvyPlan7JjuQahqmDvchYeDdvdoVjYUntaFpNUkTddSMxGwVDkJGVhyIKEr9ndimyDtkyVhZh66AeiRJjyARCSMF02nLcRdHphhGEk315xtmV41jzMcpa3i+Nax1PftWac3v1rXw5UqgVHdOstrUbT6yq+iwDzGwdsxFii65GUKcZeHEp92WziPtZ7y2IdslsI8gYp8084sIcQ3XCQTGcA3tmRoFIuD0Vz/KUO4bTjd46ikNZ68k5eEv2wgtj08qk0mZgHB2ZyIttpYmFwldk4h0sRuSAlzhCw1lCpIabyQzhZV4TIEGTcCDJnZZShpmIsURFWCvykdOQM7GgzMJNRBo0lEzERJitGxspFyIbjwh8dIBvslhGivOjFDPNPsOLacSrJQoQ9tecjWGqPI9FcGFaMvE3Th9saX572D0+JZoYWjUHfXQVTk7fq7XxVYfjlmyjcoBGujSkymeAXFmMSSP0slIDOZErhRcaZqSB6JcG6locGk8r5bwpQNqdicl6Lt0XQN1dO8/6jvM0OwXFU/Ze4Nf0ezHCFLy2KYxCWWwRsioQtxKmxSsj4YJWhxDDjim14SVYY4iQ1xG4BIt0HML8br2ZyHrbYkdqLYfUnPNF2nLfpeIzXNw3Lryt3Y1U4hhyEQPWZiwhzDjk02UM5EN4E+cmgkdQKX8PtZWQMeRiDXEbhpI74he5f37osaD2VZiNz6qYrmmZg6vben3dg1NuG1ZPxiB3JGD2JJqlsB0yYBQWLkuNsT0caP+Sxh1lKnUYUUYTaxvcWN+m/YvOSPV3L0PboXX8r0domNvljJpQddpRu2qENbZ4vZLaH9eDQtcen0S8q9eR3Gyak2CG+qwjOIJisFMLS5kpwuicLoveDFs78tdlyqn1DzXfdmT+l6P0FpS47gqv5eLLq2r7RpM9sKCyBlaZFMrToubKsAKo1Ta0SSSI9Co9ePgZhhecYyQtcAHFrg05OIMHocloP5FvLzztwdWBBoq2aj3TvVW2dXa4lOdInd1OgdpQ8bsCyMQslZjqyOix2QVFcEdwe6EXADJdw6NkksIZ3BGRXdOk6pNiAGk4sJLbaTldS2eiqR6Ij0RHoiPREeiI9ER6Ij0RLq9IoJV7lPgL8XGUv58e3ROB3VNqUyh/9Z2BhpS1fFSPih1bXzxn6/T5oxnH1cRhRaG/+s/8AmDwb8EstXBufVcg6t5oitdFt+eiO8qyJWyqTrO0N9LMTyN9S9jkL1M7KXBfN3WbevnYl00t6xrrOJNpdhdj/AJDGzjcePbE/N47VecWIun/sGnGfokYYiN5mNe2ylHD2Nzdxd5HfcFkl896QuG4gdMauvfL/ADpPazgAiNqVeP5dssvuyOocaLWHFvUaXiiDJTcYVYw2q0QjcwqTYkSEELZm3b85KqHuZRIc4MmHOk2OK08x93a0KPfnvbtM2325UNg68tfFRR1+8Q3bJGy6TwhzfPc7UnXMpJ6SsVuTqbZiS5WTE2XcajIOx7bs02zHPgqjotgyMbUmJdcK0tIYZD4FZkF7g4kYhcCLA5663MpiLi7Vdy3f7ZKm6e17Grlr7srx67YptNhk5+29MWWfgb6DBxTCl5QnD8qe8MEKpa0NZdJaUtxLWcrwWd5A4jFoKjT3ETldQlbf7c0h1d4RuPPE5o5V9t/kELN5U0LZObc6zv0ZdteWXSV0rf8AHM9eHpOuCQcJVgG6rkhySVLrdAClhipZiPZjZ9UUVwY5tV1R0BkOIdIggi0QZ8Ms8lknprpjRXKWy/c46D3/AHNevdsdR631exz1UJSqW42Q3CwXzBcK8yfU34mBkQCYpmXmQ8ScoYaLHRLTp5kgUyxFy7oZQ1rnN4ctEhjvSMgAekDeSNAfDNa8766dpOecudOWdkUPhDTkdA+FDRNsqG/ereV5ffO699zlp1SoOI07zhPw5cK9Rzq5KShrcHLIImTq/dUmTosY82ytlqPDn2RGnzZdBvpveC9w8++WtcA1oDvWcNbdARaV6LSpUhJs8XvyLpsg/n2tvXAeHi/vlOqEBM2jHgC4W5halsCgMhhis4zlLYqBWW04byzjMo6A52n/AJLNRnYn3+1e6mYrmrXmhfaz716MqWqIfUo8DdYTeey9kUaAkqu/XmdSQrtIhNjzElCHtyUSPKrkDKxGzqiQgykHSUeyx+OUS2XIxE8S1pJMggNMGxOIi4yGe4tfJdF3HX5bWnVXb3h7oAUhE1Hy4ds8Q9B6cfr8c+JAt6z2vLPz3Us+IQKO1HDBQ1v17AIcYbcaxiIDJyhn7Q63Ex853+dht3Kad2trGD5plRrpN5HqCM8jnuYKcq5f6o5t3ZrbZxWh5gpOveV9jXrm66pkKxNVUKqWjRAEcDaIWMYmQQsyMHAx6wmxJeNQ9GkstqQM7lTDiESsjmuaRizcA4a2OSRAnNOeRbpfxzdy+SjXzXNI+gNvdnXHyGAv2YXaCesI1znPYUtBVEuhmxwaNejVKoV8SytACSuVSC4b+JXBChlnx6Gx38dvd89VtxUmvZT9OQxrCPRw+kLzJmSYOm0arZnvToXkKi9HieS3TV75J6y2r0/qTlGb6M8ZXUWmU7Rutsj7rrzX7NJP5nt51JsxVUubVZn4sw+Cj3ExGCBDpCRdsLg4FQGjwPybe7rmbLhgfhNOHtDXOw1GEgC5s+ItMwbG+117Xr2+8wcfeSy5bz0wby52pcOkt+6WgumvG3vnRoF26l1Jss8KLEYl+a9im0eXfiDKz+YMY/WMyDNcii8DiMtzaB4t2ujGt+7sHfvqjA51OHBzA1rsNVroYW3kOGuuVzJtnONunOotE89aI9ypyVuO4uUbonf3Zew75pzV8hVbYTOX+nXMipTUBa4Z+OgjItuBejI0ybLmZE8GPAjlMHkEpQaLl4Ld58f79ui6a1z3cO9oloYGkyLESIPaFttyJoWubd8i3bEq1q+n3LdWufEJwnO84WGzVaIl5rXO4T+WKlG1W0UU+bDIRV7WzNCxKAbBH/iyke6Cj8Uwf7D2PQa+zlYfMKt7iGMEnCalTEATBbikjmOSjI8Y+u9Q3C2eMypyXWPFXM3WGgeo4qZmdWhcj7xpvel0v6rxMRd20zvPcJc8RDXYa9QxDsekuXigoCOAJjI5eARxpuIKlW1MUVSGPc0t9YvaaYAAgtEaWyJNpGi8ZbJ7kus+PA7n/ZVco0R5bo7yrNSu4oq365Iz01NjP9BGS5cyNdpCuOz0vUzKeXFryuKn8wp6kmOoHeKKW6XzoRrEb3jf59q6aKmPE2fNGl6MGGeqARnGcxyhWDOi+r9LdG3PoSg6rnJaXsvL+1idL7fFkq3NQTENfhI9mTfj4wyVEGGnwkivoUmTinCQnM/TKHVNraW50sBaWhpI9YSOi2Q9Fyj0RHoiPREeiI9ER6Ij0RcJcbHOHsSrgATkoMw6KNJLFYWeOK+rCnhmDFN5IaYeUlKnWW3EtuKTjK05zjHoi4mK9AJmVWPEHD4sKxsBrnsRgWJlYmMYxgVUphj85Q2MYxjDGX8tYxjGMJ+mPREOV6AemGbC9Bw7s+OKsEeccjQlzDATnzw4GzJqZya0K5hxzCx0PpaX81/JGfkr6kXFAp9Siktoi6vXY1DSDm2kAQkYGltEnlGZJDaRxm8IRIZbbyclOMYLy2jJGHPin6EXchhBxwrAMeIMCEK2lkUMNhoUUZlGPohpgdhCGWW04/ZLbaEpTj+mMeiJXXyCa9r2tfONw7tXW5Nu15e94f2WVvcU1Q9h7ApzWzq9Xr++BGQV4ha5Zo2AskOkGPCBOjZSLJDlwhmhJdk4dP2/RaGGaTwYIbJEgWJboc0zybW67JnNyklAQshJtAkRjUibFAlnNxpiXEFx7Zb7DhCASkPPJIES5hh9LriXG1YWrGSzr4fq1ZKcinSq7BEuwTamYNx+Ij3nIZlbSWFNRS3B1Kjm1MISypAmWU5aSlvOMoxjGCL9W67X2fsYZgodr8WNchhvtxgSPx4h76ZdimPixj7Ma7lKfuAt/EVfxx8ms/THoi/E6rViTixoOSrkDIQgSh1hw50RHlxYixE5QKoaPIHcEYUMhSkjqaZRllKspbynGc49EnXVIt+K7oTdXZvuG9x1vqDY9k3JX+NLl3AvmKFtJLSIvUK39jG66UuvBRLEaOWtulobgRVzrcsoMZlh4TLBg7JLZbHgN4cYRGMU8XO0+N09oNFxobRTAkcCKwc+QUayMIOw0YUX/wB7JKbabSgh8r6Y/Iedwtx//wAxSvRY18MRESLG/ow0XHDw+B3RcRLAQzMb+K/heHxvwW2ki/jvYccw6z9r7bmHF/NOfkr6kXU/wTTPzo2T/hGsfqUO2w1ESH6BFfnRTQyfiM3Gl/iffBbHT/KwgZxpLSf2bwnHoi5CqnVlzbdlXWoBdjaxlLU+qGjlTbeFN5ZVhuVyNk5GMtZy1nCX8Yy3nKM/y5+noi/plWrEia/JSFcgTpEqPXFEyBkRHkmkRbn1y5GvlPjrfdj3M5zlYbjih1fXPybz9fRFzx4uMEJdNFjgBjHxxhHyxxB2SXhQ8KSGM6+22l1wcRK1pGZWvLbGFqw0lOFZ+pF1mKhUsTarNir13FjXnGV2DEJG4m1ZS3hpOVSv4356spaxhvH1fz9G8YRj+XGMeiL7eqdWJmm7IRWoB+xMpbQzPPQ0c7NNIax9GkNyixlHNpbx+zaUv4wjH7Jxj0RdmLHx4ThjwQIYj0gR+We6KMyO4cXltDWSjFtIQokjLTbbf3nsrc+2hCPl8Upxgi5noiPREeiI9ER6Ij0RHoiPRF//2Q=='
                                            } ],

                                            // contract Rates Header
                                            [
                                                {
                                                    bold:true,
                                                    style:[ "fontSize_12" ,"mt_40" ,"m_l_50" ],
                                                    text: "Contract Rates"
                                                }
                                            ]
                                        ],
                                    },
                                    layout:'noBorders'
                                },
                       // Customer and Contract Info
                                {
                                    style:[ "fontSize_8",'mt_20','m_b_5' ],
                                    table:{
                                        widths:[ 100, "*" ],

                                        body:[
                                            [ {
                                                bold:true,
                                                text:"Intertek Contract Holder:"
                                            } ,
                                            {
                                                text: contractHolderCompany
                                            }

                                            ],
                                            [ {
                                                bold:true,
                                                text:"Customer:"
                                            } ,
                                            {
                                                text: customerName
                                            }

                                            ],
                                            [ {
                                                bold:true,
                                                text:"Intertek Contract Number:"
                                            } ,
                                            {
                                                text: contractNumber
                                            }

                                            ],
                                            [ {
                                                bold:true,
                                                text:"Customer Contract Number:"
                                            } ,
                                            {
                                                text: customerContractNumber
                                            }

                                            ],
                                            [ {
                                                bold:true,
                                                text:"Effective To Cut-Off:"
                                            } ,
                                            {
                                                text: cutOffdate
                                            }

                                            ],
                                    ],
                                    },
                                layout: 'noBorders'
                                }
                            ]
                        ]
                    },
                    layout:'noBorders'
                };
            },
            footer: function(page, pages) { 
                return { 
                    columns: [ 
                        { 
                            alignment: 'left',
                            style:[ 'fontSize_8','mt_20','m_l_50' ],
                            text: [
                              { text : 'Run On '+ moment(new Date()).format('DD/MMM/YYYY HH:mm').toString() }
                            ]
                        },
                        { 
                            alignment: 'right',
                            style:[ 'fontSize_8','mt_20','m_r_30' ],
                            text: [
                                { text: 'Page ' + page.toString() },
                                ' of ',
                                { text: pages.toString() }
                            ]
                        },
                        
                    ],
                    margin: [ 10, 0 ]
                };
            },
           styles:{
            fontSize_10:{
                fontSize:10,
            },
            fontSize_8:{
                fontSize:8,
            },
             fontSize_12:{
                fontSize:12,
                marginTop:20
            },
            m_l_5:{
                marginLeft:5
            },
            m_t_40:{
                marginTop:40
            },
            m_t_50:{
                marginTop:50
            },
            m_t_b_5:{
                marginTop:5,
                marginBottom:5
            },
            m_r_30:{
                marginRight:30
            },
            m_l_30:{
                marginLeft:30
            },
            m_b_5:{
                marginBottom:5
            },
            mt_20:{
                marginTop:20
            },
            mt_30:{
                marginTop:30
            },
            m_l_50:{
                marginLeft:50
            },
           },
           pageMargins: [ 50, 120, 40, 60 ]
        };    
        
        scheduleRates.map((item,index) => {
                item["scheduleNameWithCurrency"] = item.scheduleName + " (" + item.chargeCurrency + ")";
                if(index == "0")
                {
                    documentDefinition.content.push({
                        style:[ 'm_l_5', 'm_b_5' ],
                        table : {
                            widths:[ "auto","auto" ],
                            body: [
                                [
                                    {
                                        bold:true,
                                        text:"Schedule:"   
                                    },
                                    {
                                        text:item.scheduleNameWithCurrency
                                    }
                                ]
                            ]
                        },
                        layout:"noBorders"
                    });   
                }
                else
                {
                    documentDefinition.content.push({
                        style:[ 'm_l_5', 'm_t_b_5' ],
                        table : {
                            widths:[ "auto","auto" ],
                            body: [
                                [
                                    {
                                        bold:true,
                                        text:"Schedule:"   
                                    },
                                    {
                                        text:item.scheduleNameWithCurrency
                                    }
                                ]
                            ]
                        },
                        layout:"noBorders"
                    }); 
                }
                
                item.ChargeRates.map( (iteratedValue , index ) => {
                    //console.log("index",index);
                    if( index == "0" )
                    {
                        const header = [   
                                                {
                                                    bold:true,
                                                    text:"ChargeType", 
                                                
                                                },
                                                {
                                                     bold:true,
                                                       alignment:"right",
                                                    text:"Value", 
                                                
                                                },
                                                {
                                                     bold:true,
                                                    alignment:"right",
                                                    text:"Effective From", 
                                                
                                                },
                                                {
                                                     bold:true,
                                                        alignment:"right",
                                                 text:  "Effective To" 
                                                },
                                                {
                                                     bold:true,
                                                    
                                                    text:"Description"
                                                }
                                                
                                            ];
                        body.push(header);
                    }
                    body.push(
                              [
                                   {
                                    text: iteratedValue.chargeType, 
                                },
                                {
                                        alignment:"right",
                                    text:parseFloat(numberFormat(iteratedValue.chargeValue.toString())).toFixed(2).toString(),
                                },
                                {
                                        alignment:"right",
                                    text: moment(iteratedValue.effectiveFrom).format('DD/MM/YYYY'),
                                },
                                {
                                        alignment:"right",
                                    text: moment(iteratedValue.effectiveTo).format('DD/MM/YYYY'),
                                },
                                {
                                    text:iteratedValue.description
                                }
                            ]
                    );
                    documentDefinition.content.push({
                        style:'fontSize_10',
                        table:{
                            headerRows: 1,
                            widths:[ "*","*",90,90,"*" ],
                            body: body
                        },
                       
                    });
                    
                    body= [];
                });
        }); 
               
        return documentDefinition;
};