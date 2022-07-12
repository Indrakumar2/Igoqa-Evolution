import { getlocalizeData } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
export const sideMenu = [
  {
    "menuIcon": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAQAAABpN6lAAAAACXBIWXMAAA3XAAAN1wFCKJt4AAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAASbSURBVHja7J1PaBxVHMc/u0mqIFKKxT+4B60FQ7HkYC6xYLYYUKKolxowCHYvCrYHFeuxY+hBRcGDnjyoSKyKfyhqUVAaRRAlxYoYIhQVU7EUq4lWNJu2Py/j7DCZ3XkzO7udee/3zSFvJ2/evPfZeb/3fr/fDEHI7Wda5qWzVuV12R46Y0q+lK/l/tCR6+Rl+SehlXmZzq/X+Q2/JmfFRHPBGZdLU0REzsuW4NhbRm2clVpe/a6Sl6YYMKp3Mxv90m0MAVDhDv/IELcbtTHAVF7dzg/ApYb1PmPFL33IGgDC+/6RNT7I+Wp9BGCiJm+wN/h0ivv4imM0+CE49jiv8K9BS5Jbn3KzAV4wQ70cDWvPr9HfO6CAUgAKQAEoAAXgsipd7Cg2McYIF/mf6oz7pU+Z61Fvx6mvu8Yq3/AFf/R/I9SQZSmKlqXRb2+wIUVTRgTZpsAmfgx8uqJohWuzTIRsRnCscMOHjYxlOW0w08VGgpKJwdvMQ37pNM8btL6Hy/zSC/yWWLtlfEc43C8jmM4rGw5qLxq1vhjUH+69h6gbIQWgABSAAlAACkABKAAFoAAUgAJQAOXWAwy7DeBKjqRHYNcUyIBgsMtL1vES62wOlTyDNlv19xiFxKIIdrJoPoBsUWGP/QW+D06mQWDjKpBqIti5DKZAYBOAWU6kR9CtESxSXuA4HkeopTSHluUFtspSKF/4a3ILttmA4+xMNxHsM4IpEdi4CqRCYOcymAKBre6wMQJ74wGGCGwOiBghsDsiZIDA9pBYIgL7Y4IJCFwIinZE4EZUuAMCV8LibRG4kxdog8ClxEgsArcyQ+sRbHEtNRZFsG8QqDFl9CbmPIc5HzlWpLxAtC8rHORkLIJW4GwamTZ86VlE5DupRsJQxVZT7ooNhG1tVanysOFLzwDbmCzVDT/Eo20mQmgjdCNOK50RXMj0QPqF0xrPJlUJ5wWeyGAEi5QXiPYl3ghGFTIZLr0vEDKCqA1QAApAAcSqwp28ywxX2wygU3p8gkPA3dzCjlKMxcwXSAGg4f++iev5vgQAxoMdwf96il0cyj4FrooplUvtfAGHjKC4DWCVp7uxAWVT1BdY5jVOuQRgzijepBshBaAAFIACyG8VKFJeYIKLI8vgq/ySvFVqHxKbC/5SbxuGKrbW2uQFnAmJDfKY6zag6bov8EyvjWCR8gKf8HEkIPImp3ttBK3OC/wVU3JqIzTr/17kqJsA3mE/P/Eeu13dCTaZYUZ9AfUFSqOoL7DCLD+7BGDHugTOAe7h7W6mwBXs41YqJZ7gj3RzB9zAUTYAB7m3xJvhLozgg2wAYFdpM0Nd+gK/B838XYrhZvIFOgF4kd3UOMdz/FkKAJ/zZL7L4BLXMMECS+7uA87xke4EFYACUF8AqDLJaPCpHip5iedeiPcF4h7rjVVFWs9QVDoM/1u2leyrXWB7BwSSbgpMlm74xu82mAEYLeX0Hs0PgBrBQL37Byp5qb7uacFcAWR6Dqev8tIB0I2QAlAACkABKACHFXaG9A5wE8AZp8d/pho8BuGmZge8Y1zCsJ8Dcuzb5yUO/DcAq+Og3t/u7l4AAAAASUVORK5CYII=",
    "menuText": localConstant.header.COMPANY,
    "module":localConstant.moduleName.COMPANY,
    "name": "mnuCompany",
    "subMenu": [
      {
        "module": localConstant.moduleName.COMPANY,
        "name": "mnuViewEditCompany",
        "menuText": localConstant.sideMenu.COMPANY,
        "viewUrl": "Company",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.sideMenu.COMPANY
      }
    ]
  },
  {
    "menuIcon": "data:image/png;base64,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",
    "menuText": localConstant.header.CUSTOMER,
    "module": localConstant.moduleName.CUSTOMER,
    "name": "mnuCustomer",
    "subMenu": [
      {
        "module": localConstant.moduleName.CUSTOMER,
        "name": "mnuViewEditCustomer",
        "menuText": localConstant.sideMenu.CUSTOMERS,
        "viewUrl": "Customer",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.sideMenu.CUSTOMERS
      }
    ]
  },
  {
    "menuIcon": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAYAAADDPmHLAAAACXBIWXMAAA3XAAAN1wFCKJt4AAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAKjSURBVHja7N3NTiMxEEXhGPX7v3KxYjESjNOhm7h8v7OceCLadVw/ThCjqh43cMubbsJY6Yf5EPw/p3YXAI0kIEC4BEdi3Vso0PXuvZEBwjMBAcIlIEC4BAQIl4AA72GsIgEBwiUgQLgER8NNq5Mb2UGCetc9wccGwf/695IJ9hagLlpDgoYlYLdPGOvF/zOSM0CqLLc+W2cBxiP7Q6ZoAQQ+eAxMkrVSBagLX5ctwprApKaQACAAAprAM3Vcym+eAQRQCQABQAAQAAS4dWIggC0gQAJGy8YCSOkyAO5itavgkr5lAAQLoN7LAMRK7gEESwYAAUAAEAAEAAFAABAABAABQAAQAATAVaz4aeDu3wAaMgAIEHr6l3tGXwlTAmxQ8rMdNkoPAAKAACAA8nATaAyEEuD0xz6jm0AlwAYlP5ubQD0ACAACgAAIxE2gMRBKgNMf+4xuApUAG5T8bG4C9QAgAAgAAiCQ5JtAjaYMgOSbQH+J5JF9E6gEPHJvAgV/4SZw5QDVbmIdzsCl/UJ1E4EA9zSKbUQwBt47JRQBcoPfQgIl4LpGrybvOWSA3sEfkyDOXi8C9GX8ENDqPgoqAfMTOiZr6pu14z9rfSew6cmvkxK1yAQECIcA149yRYA9AjxL4ePJNUsLQgAlAATAK6m6nlyz9D0BAV4X5Iq1BAABukwH4+TpbzEOugr+N3iz69vvAnymfxgywF73BMvP+QS4rtmbdf2z15dsDpWA50vBb074kAH2Hftaj4UECOevSkAFnvwW+yADyAD4YeZ/JUNUp9N/Vwbo/IuXY/H3Hh0E6CrB6r+VfMvPdzTfUFlGD/C2XmEL2U0B14xyRYDc4LeWgADhEIAAIABeHefaj7rGwN/P8UMTmBHo2bx/dv0SfA4A3KZvNZqSIAQAAAAASUVORK5CYII=",
    "menuText": localConstant.sideMenu.CONTRACTS,
    "module": localConstant.moduleName.CONTRACT,
    "name": "mnuContract",
    "subMenu": [
      {
        "module": localConstant.moduleName.CONTRACT,
        "name": "mnuCreateContract",
        "menuText": "Create Contract",
        "viewUrl": "CreateContract",
        "menuFun": 'CreateContract',
        "currentPage": "createContract" //refer from locale constants
      },
      {
        "module":localConstant.moduleName.CONTRACT,
        "name": "mnuViewEditContract",
        "menuText": "Edit/View Contract",
        "viewUrl": "EditContract",
        "menuFun": 'EditContract',
        "currentPage": "Edit/View Contract" //refer from locale constants
      }
    ]
  },
  {
    "menuIcon": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAQAAABpN6lAAAAACXBIWXMAAA3XAAAN1wFCKJt4AAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAATqSURBVHja7JxrbBVFFIC/YmMrgVSNWrUovfhqDGAippfEH9SQJvaP+EoUieIPDEQlJcZXb98gDyEmNDEY4yOVRMoPEV9J4wuqxthEStJoStESFRACSCIStRXa4w+2m9nbll5u56a7O+fMj85sz527882cszNnZm+eMKaU8gthkQ3U5KbiKURDXmS92wByhiA6AHKEID8jrcc5NmnNbg8gEFKW65exU6kMS6kwaSld1tmtP0omcE5qWOemD9iVGwTRAbCJZgPBWhefAk0GgpQtBNHyAU2sto0gak6wMYDgJfcABBHUThxB9ABAI2vsIYgiAGiwhyCaANIRrHEPADQYPV+XPYLoAoB6GwiiDMAKgmgDgHpjOlRnPCCdAQB1BoL6C0cQfQATRBAHAFBnLJAvEEE8AEBtAEGzewCg1giZNmSOID4AIJUNgjgByApBvABAig0Ggib3AECNgaBxfAT5kWlYMaUZar7OdSzxEXB+CNEB0Jrl58ZBED8TGA3B9W4DOG8r3QAQaR+QyPqTM/gmDgB+nTTrcEMUQM6/4RIe5AWKXHWCN7KT2cByZrk4Aqr4ntmeL7/FPQApPuFSL99DX5wBXMm8tCvTeI+1fu0HeYTB+AJYzCH20EmxYfmdPOCXOriD7jg7wWcoAJLs4i6OA1Vs84c+tPAsZwMzu4k/EQbYZ42AhXOC23y9H+UqqZVBv/yvPBrQnCZfiR3ZJ3PGPQOY0f3bAHCrnPQ1/zJu8jeZl6a5QuxJqx0ANnxADws56eWnByy/K03ztEXjPR0eE0CQ2+SPQP+0SP4oWgXyjvRb6P0haZeEnRGQl9ELE4kM1mRz+ZIrAOhnOVvH1CukcMK9doa/M9DK7P4tHpaeI50i0j3C8icrZXT/NtcCPzCf4kk8WB+KqXDEmq/xAAWgABSAAlAACkAB5FqmugzgCfo4kat3v8OzHB49XSxveDWckaL4L4bS5Vp2MN/LDzGdU26ZwJ10+c2HjRyOrw+YwRY+597AtRXs5movf5Zq6uPsA972tJ73A19vGuGr41IR94DI7d7flxnkFUrYQdL/Xxf3c9DQrWKpsWeQrQzwPm38F5YR8KTR35vlqFHaKoUBzaTFsHh1eMLir7HFz1cblr+Kx+gPaFZatN3K8DhB4SleTbt2gkpaRmhut7hF+q6damzNA1YirPRLe7kvYPnD0keCJZZ8QHd4ngLDafMYlu9MWHwVH7CIzwK/+xJ6sTsV7qBDl8MKQAEoAAWgABSAAlAACsCTIpax3jhFGuup8EiZy05mAUspQdwbAQ/xnfeewDXuHZe/iI1s9zfF9rI/zgASLCQvcOVy2nnOL+1ncTgNwA6AZfTxBT3MNCx/jxGz+4hyfoqzE3yaKUAZu1nAIeBh3vKHvrCaZqP380haCYl9aykobnVjROSAzJRNRuj6lNwT0LxMei0FxY9JMjzH5W8w9gLMo9C9UpamWW1xX6AtPPsCB6jgqJcv8K9+TDm9aZpHLBrvkfCYAILcLL8HjrM3S94oWvnSIn9a6P0BaZOS8JjAuXSTHPbfGlkUnbC4vYnQz1TwKUN8TTkfRmcxZHMt0MfdTOUft1eDEWu+xgMUgAJQAApAASgABaAAFIACUAAKQAEoAAWgABSAAlAACkABOCWZbYwsoCyCbSu2B6BVTUABKICYyv8DAEc2GDTe/BXaAAAAAElFTkSuQmCC",
    "menuText": localConstant.sideMenu.PROJECTS,
    "module": localConstant.moduleName.PROJECT,
    "name": "mnuProject",
    "subMenu": [
      {
        "module": localConstant.moduleName.PROJECT,
        "name": "mnuCreateProject",
        "menuText": "Create Project",
        "viewUrl": "Project/SearchContract",
        "menuFun": 'CreateProject',
        "currentPage": localConstant.sideMenu.CREATE_PROJECT //refer from locale constants
      },
      {
        "module": localConstant.moduleName.PROJECT,
        "name": "mnuViewEditProject",
        "menuText": "Edit/View Project",
        "viewUrl": "EditProject",
        "menuFun": 'EditProject',
        "currentPage": localConstant.sideMenu.EDIT_VIEW_PROJECT, //refer from locale constants
      }
    ]
  },
  
  {
    "menuIcon": "data:image/png;base64,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",
    "menuText": localConstant.sideMenu.RESOURCE,
    "showMenu": true,
    "module": localConstant.moduleName.TECHSPECIALIST,
    "name": "mnuTechSpecialist",
    "subMenu": [
      {
        "module": localConstant.moduleName.TECHSPECIALIST,
        "name": "mnuCreateTsProfile",
        "menuText": localConstant.techSpec.common.CREATE_PROFILE,
        "viewUrl": "CreateProfile",
        "menuFun": 'CreateProfile',
        "currentPage": "createProfile", //refer from locale constants
      },
      {
        "module": localConstant.moduleName.TECHSPECIALIST,
        "name": "mnuViewEditTsProfile",
        "menuText": localConstant.techSpec.common.EDIT_VIEW_TECHSPEC,
        "viewUrl": "EditResource",
        "menuFun": 'EditViewTechnicalSpecialist',
        "currentPage": localConstant.techSpec.common.Edit_View_Technical_Specialist, 
      },
      {
        "module": localConstant.moduleName.MYPROFILE,
        "name": "mnuMyProfile",
        "menuText": localConstant.techSpec.common.MY_PROFILE,
        "viewUrl": "ProfileDetails",
        "menuFun": 'EditMyProfileDetails',
        "modalPopupCallback":"confirmPopupForProfile",
        "currentPage":localConstant.techSpec.common.Edit_View_Technical_Specialist,
      } ,
      {
        "module": localConstant.moduleName.MYSCHEDULE,
        "name": "mnuMySchedule",
        "menuText": localConstant.techSpec.common.MY_SCHEDULE,
        "viewUrl": "mySchedule",
        "menuFun": 'HandleMenuAction',
        "currentPage":"My Schedule", //refer from locale constants
      } ,
      {
        "module": localConstant.moduleName.MY_ASSIGNMENT,
        "name": "mnuMyAssignment",
        "menuText": localConstant.techSpec.common.MY_ASSIGNMENTS,
        "viewUrl": "myAssignments",
        "menuFun": 'HandleMenuAction',
        "currentPage":"My Assignments", //refer from locale constants
      } , 
      // {
      //   "module": localConstant.moduleName.MY_DOCUMENT,
      //   "name": "mnuMyDocument",
      //   "menuText": localConstant.techSpec.common.DOCUMENTS,
      //   "viewUrl": "documents",
      //   "menuFun": 'documentsDetails',
      //   "currentPage":"Documents", //refer from locale constants
      // } ,
      {
        "module": localConstant.moduleName.PREASSIGNMENT,
        "name": "mnuPreAssignmet",
        "menuText": "Pre-Assignment",
        "viewUrl": "PreAssignment",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.techSpec.common.PRE_ASSIGNMENT_PAGE,
      },
      {
        "module": localConstant.moduleName.QUICK_SEARCH,
        "name": "mnuQuickSearch",
        "menuText": "Quick Search",
        "viewUrl": "QuickSearch",
        "menuFun": 'HandleMenuAction',
        "currentPage": "quickSearch", //refer from locale constants
      },
      {
        "module": localConstant.moduleName.TIMEOFFREQUEST,
        "name": "mnuTimeOfRequest",
        "menuText": "Time off Request",
        "viewUrl": "TimeOffRequest",
        "menuFun": 'HandleMenuAction',
        "currentPage": "TimeOffRequest", //refer from locale constants
      },
      /** GRM Reports Starts */
      {
        "module": localConstant.moduleName.TECHSPECIALIST,
        "name": "mnuCalendarScheduleDetailReport",
        "menuText": localConstant.techSpec.common.CALENDAR_SCHEDULE_DETAILS_REPORT,
        "viewUrl": localConstant.techSpec.common.CALENDAR_SCHEDULE_DETAILS_REPORT_PAGE,
        "menuFun": 'HandleMenuAction',
        "currentPage":  localConstant.techSpec.common.CALENDAR_SCHEDULE_DETAILS_REPORT_PAGE,
      },
      {
        "module": localConstant.moduleName.TECHSPECIALIST,
        "name": "mnuCompanySpecificMatrixReport",
        "menuText": localConstant.techSpec.common.COMPANY_SPECIFIC_MATRIX_REPORT,
        "viewUrl":localConstant.techSpec.common.COMPANY_SPECIFIC_MATRIX_REPORT_PAGE,
        "menuFun": 'HandleMenuAction',
        "currentPage":  localConstant.techSpec.common.COMPANY_SPECIFIC_MATRIX_REPORT_PAGE,
      },
      {
        "module": localConstant.moduleName.TECHSPECIALIST,
        "name": "mnuInstantSearchProfileReport",
        "menuText": localConstant.techSpec.common.TAXONOMY_REPORT,
        "viewUrl": localConstant.techSpec.common.TAXONOMY_REPORT_PAGE,
        "menuFun": 'HandleMenuAction',
        "currentPage":  localConstant.techSpec.common.TAXONOMY_REPORT_PAGE,
      },
      /**Changes for WonLost Report SideMenu */
      {
        "module": localConstant.moduleName.TECHSPECIALIST,
        "name": "mnuWonLostReport",
        "menuText": localConstant.techSpec.common.WON_LOST_REPORT,
        "viewUrl": "WonLostReport",
        "menuFun": 'HandleMenuAction',
        "currentPage":  localConstant.techSpec.common.WONLOST,
      },
      {
        "module": localConstant.moduleName.TECHSPECIALIST,
        "name"  : "mnudownloadedreportstoview",
        "menuText": localConstant.techSpec.common.DOWNLOADEDREPORTSTOVIEW,
        "viewUrl": "downloadServerReport",
        "menuFun": 'HandleMenuAction',
        "menuFun": "HandleMenuAction",
        "isReportMenu":true,
        "currentPage":  localConstant.techSpec.common.DOWNLOADED_REPORTS_TO_VIEW,
      },

      /** GRM Reports END*/
      // {
      //   "module": localConstant.moduleName.MY_CALENDAR,
      //   "name": "mnuGlobalComponent",
      //   "menuText": "My Calendar",
      //   "viewUrl": "MyCalendar",
      //   "menuFun": 'HandleMenuAction',
      //   "currentPage": "myCalendar", //refer from locale constants
      // }
    ]
  },
  {
    "menuIcon": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAQAAABpN6lAAAAACXBIWXMAAA3XAAAN1wFCKJt4AAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAANlSURBVHja7N0/SBthGMfxb2JVNEjRQaRFKxREUBCsoLg4OTVUSjsIWpwEoSlYtIs4GYpDxaUU0UGQDl2KRS10FgoiWB1EELHVIO1QXUTFoTbpcokmxqJ3Xtr38nuWS4j35J6Pd2/u3/ueL8a5qKafBmq4gXfihDWWGGE99QPfOYCnvKIAb8YxL3jzd4AHzODtaGP2YoCbbFDqcYCfVLF/+jZ5O29OlP+Btxx7qOwCnvAQgFKa+XQRwD1rus1joh77z8/wlUqryjMA/qQ/um1NFz1XPkRZTKkyDYCPbAjfxQBZGAIQgAAEIAABCEAAAhCAAAQgAAEIQADXG03UOs5RS5OpAFMssMqQoxxDrLLAlIkAZXQAECLXdo5cQgB0UGYiQA4AxQRs5whQDECOiQBZ3wjG/uNsGQH4nnhVbjtHeZpsxgDssWO96rKdIz7nDnsmtgFfrGkvAzYawgADPE/J5EK4eRdImPvkAjm8ZIgfV7re6OeW9SsCvwibCbBMOLEblOOgJQizbOqu8DCjDq8zRxll2NxjgRP6aGHFJkKUFVro48TNRXT/TrDP1FNEPVWJbfoy8ZsNljlwf0coM7fCHTDPvM4HCEAAAhCAAAQgAAEIQAACEIAABCAAAQhAAAIQwD+P9GeFA1YPO29F4PIAQYLaBAQggCxuBD/yzIO1vk7XsqUHOGLbgwBH2gQEIAABCEAAAhCAAAQgAAEIQAACEIAABCAAVwHyaCfo8Bv8BGknz0yAOd4xx5ijHGNWFgMBKmgFoJN82zny6QSglQrzAEqsoSsLHQzTXEAhAD5K1AgaB3DaVc7+UK2+NNmMAdhKdHm/aztHfM4YW+YBHLBpvQrZzhGfc9O9HoRutgHxgYy7mKD6it/kp5qJRPf5RfcW0s2Ok4O0UQRAN92O1qVBM3eEIvReS55eIqYeC0zSw6GjDIf0MGnywdA4dUyza2veXaapY9zdBXS/8/Q3HgF3bPQej5CByNSDVCKZKUe7wgIQgAAEIAABCEAAAhCAAAQgAAEIQAACEMCVAGJZUXPsYoD4ANaNHlwz/DSmVAmknhWOj+BcyXsPPna3MqVKQA9eTlnV9x1dxDQjus+Wf/5XYJaQp1b95DgmlPzk8XRPn4dq+mmgJmNXjTIRJ6yxxAjrqR/8GQCub5j/FLnrDQAAAABJRU5ErkJggg==",
    "menuText": localConstant.techSpec.common.DOCUMENTS,
    "showMenu": true,
    "module": localConstant.moduleName.TECHSPECIALIST,
    "name": "mnuDocument",
    "subMenu": [   
      
      {
        "module": localConstant.moduleName.MY_DOCUMENT,
        "name": "mnuMyDocument",
        "menuText": localConstant.techSpec.common.INTERTEK_COMPANY_DOCUMENTS,
        "viewUrl": "IntertekCompanyDocuments",
        "menuFun": 'HandleMenuAction',
        "currentPage":"intertekCompanyDocuments", //refer from locale constants
      } ,      
      {
        "module": localConstant.moduleName.MY_DOCUMENT,
        "name": "mnuMyDocument",
        "menuText": localConstant.techSpec.common.INTERTEK_OPERATIONAL_DOCUMENTS,
        "viewUrl": "IntertekOperationalDocuments",
        "menuFun": 'HandleMenuAction',
        "currentPage":"intertekOperationalDocuments", //refer from locale constants
      }       
    ]
  },

  {
    "menuIcon": "data:image/png;base64,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",
    "menuText": localConstant.sideMenu.SUPPLIER,
    "viewUrl": "supplier",
    "module": localConstant.moduleName.SUPPLIER,
    "name": "mnuSupplier",
    "subMenu": [
      {
        "module": localConstant.moduleName.SUPPLIER,
        "name": "mnuCreateSupplier",
        "menuText": localConstant.supplier.CREATE_SUPPLIER,
        "viewUrl": "CreateSupplier",
        "menuFun": 'CreateSupplier',
        "currentPage": localConstant.supplier.CREATE_SUPPLIER
      },
      {
        "module": localConstant.moduleName.SUPPLIER,
        "name": "mnuViewEditSupplier",
        "menuText": localConstant.supplier.EDIT_VIEW_SUPPLIER,
        "viewUrl": "EditSupplier",
        "menuFun": 'EditSupplier',
        "currentPage": localConstant.supplier.EDIT_VIEW_SUPPLIER
      },
      //added for Reports
      {
        "module": localConstant.moduleName.SUPPLIER,   
        "name": "mnuSupplierVisitPerformanceReport",
        "menuText": "Supplier Visit Performance Report",
        "viewUrl": "visitPerformanceReport",
        "menuFun": 'HandleMenuAction',
        "currentPage": "visitPerformanceReport",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'visitPerformanceReport',
      },
      {
        "module": localConstant.moduleName.SUPPLIER,
        "name": "mnuSupplierDownloadedReport",
        "menuText": localConstant.supplier.SUPPLIER_DOWNLOADED_REPORT,
        "viewUrl": "downloadServerReport",
        "menuFun": "HandleMenuAction",
        "isReportMenu":true,
        "currentPage": localConstant.supplier.SUPPLIER_DOWNLOADED_REPORT_MODE,
      },
    ]
  },
  {
    "menuIcon": "data:image/png;base64,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",
    "menuText": localConstant.sideMenu.SUPPLIER_PO,
    "module": localConstant.moduleName.SUPPLIER_PO,
    "name": "mnuSupplierPo",
    "subMenu": [
      {
        "module": localConstant.moduleName.SUPPLIER_PO,
        "name": "mnuCreateSupplierPo",
        "menuText": localConstant.supplierpo.ADD_SUPPLIER_PO,
        "viewUrl": "SupplierPO/SearchProject",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE
      },
      {
        "module": localConstant.moduleName.SUPPLIER_PO,
        "name": "mnuViewEditSupplierPo",
        "menuText": localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO,
        "viewUrl": "EditSupplierPO",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE
      }
    ]
  },
  { "menuIcon": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAHoAAAB9CAMAAABagoduAAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAAo5QTFRFAAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////gqK45QAAANp0Uk5TABFQh67U5/T/FW3I/ReV+gx/81roAosOtSDWluBTPOkE3Pc4tP5UGncJyZwBrbwF1Q9lRfs3K/HvVR5q8m4lz8pNBgiXfrdYimdouxbQ5kCxqvx9V6xwO6mR6qtzvpn5o1tLNN1mhGJp9qgNMzBR2qUspM3FT+6UKAuiHEFj4UMdMgeysKGAfL8hToYkLi3fA0brOpjwH8YpiNOndM7t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",
    "menuText": localConstant.sideMenu.ASSIGNMENTS,
    "module": localConstant.moduleName.ASSIGNMENT,
    "name": "mnuAssignment",
    "subMenu": [
      {
        "module": localConstant.moduleName.ASSIGNMENT,
        "name": "mnuCreateAssignment",
        "menuText": localConstant.assignments.CREATE_ASSIGNMENT,
        "viewUrl": "Assignment/SearchProject",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE
      },
      {
        "module": localConstant.moduleName.ASSIGNMENT,
        "name": "mnuViewEditAssignment",
        "menuText": localConstant.assignments.VIEW_EDIT_ASSIGNMENT,
        "viewUrl": "EditAssignment",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE
      },
      {
        "module": localConstant.moduleName.ASSIGNMENT,
        "name": "mnuAssignmentDownloadedReport",
        "menuText": localConstant.assignments.ASSIGNMENT_DOWNLOADED_REPORT,
        "viewUrl": "downloadServerReport",
        "menuFun": "HandleMenuAction",
        "isReportMenu":true,
        "currentPage": localConstant.assignments.ASSIGNMENT_DOWNLOADED_REPORT_MODE
      }
    ]
  },
  {
    "menuIcon": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAQAAABpN6lAAAAACXBIWXMAAA3XAAAN1wFCKJt4AAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAnWSURBVHja7Jx7lE3XHcc/dx4IQczIw4yQIAZLdKSSqEyQ1axIpQaNWgmqiClSomiI1KNNkMRjkiYrZYW2rKAWojLVRlhrWhV5FFmSeAdDhIwxiUc8Z5jdP5hxz2+fs+895po5Z5zf/efu3/6dfc7+nv34vc4OKSqcHuW9CrzbEW4zVcdxnVMAQABAAEAAwHVNCdek1U60NdS2EOW/8VVM7/4YrcNKtRiHKvuVhP1XKM6Jcox+ryk31DnGd5/v4t7HgzUgACAAIAAgACDQA64tnecOY/13Mb5fFsMIOfzimE7/igYA8iv0pRZTbKg9G0yBAIAAgACAAIAAgACACtUDIlFDehlqj7LIUu5GU4P0Grb7D4BmvGqo/UwA8BTdDdID3QEQrAEBAAEAAQABAIEeUNn0IQ2M9r2VBlDDIH3CjwAUuXKZHK/cEVCTdFJJJYVUUkiikHyOkE8+2/gUVZWnwC10ozsPc4OjRCFrWcOaqgdAXbLoSfuIS2Z9nuRJ4ExVAqAaw5hIsstpUkUACNGbaTS5XrfBNszj3gp5ihQyDbWFLLeUu3CnQfrf7HJ1b8co+8/UKRUbuqi6RIjodzZev0VIrzRKD4hwrznR5AeEmMxyahmRO8d+PuI9NnGICxG0zRyjw8NzU6AWC3jc4J5YySr2cszSxfq0pjs9aOSwkC5hCH/2BwC1+Q/32Mru5w3+Tp5NTQkF5JLLSO6hF09TV5OIZx43Mcv7xlAci2y7/y2jaUG2bffD6VOepymvct6mbiYveh+AqXTTZC7wsmOncAArTfjxLtEERnkbgL48Z9OdRxjv1sbiAP3IsonSTiPNWwCEwqyX+1inGZrbyGTfVbf+IO9ws+B9TAYXBa+GMaW9iMPCJqlp1BpOGZ9pDkMsxnPZ/pigdmp76mpVu5w5e43Vdq3VcYpK/Ak94ErFEO1BP1c3xuCGd6pC0e451co7ACSU7f2/14ZSpmEwVaM9DbmNunxLPnlscvQE5NGLtZbttjoL+FEE5ani6DIuE8RbKlIdHTHsqhapE0L+sHpTtXO8YqhrhbWCp8DN6qR4wEkOl2eoDQY9fLlKc7huhZD8r7cAmCIe7xtVy+bSODUrouFTpLJsb5umioVkMy+tAVLzf4HTNiryErpGnFGJvEVrRlEi+Lv4C78Svt0JZf9bM8XQZp5Qn8Zzv0H6dXLdrgFp4t3sVgkabvHqfRcG8Ewb5Buo0xaZr1ScV8zhngKTGTYrdDaPuEB1DAM13jfMt5Rv52GvqMI9hGX3rib1KM84DiB7mk1jjbdClAd6A4AU7rNwNlCgyUy3MYDn0Zc0atCWLP6h1VfnBY23zuJDgM7eACCDkIWzUpPpx92Cs5dOZLGY3RSxhXlk0ld0DvrRUrMqV1nKt1LdCwCkCo4+AX6hrejpfCB4i2nHSdFyP62ld4XjrZEXPELWsGSxZvvdRCfxHvvbqsj7GCWcXo/zOw06KzXmSwBO8ZnhGXdp26JJ2m3uuXrbsi0ctPEOWynbsMXkCtkmoj5Z1D9V+YpQHClis5IkffDvG9CUdY0050qRGAEeWAMaRAAgRZQ3G1rbJMq6myPfewAkRZhB9cU7LHQxW5MizNBbvADAQeOgRXQ4WQBipbSIC5K1/UNeAGCPpdxckzgsyj80tNbOOOAhWYyJPd4DIFVzOMpIQBdDa7JOfhR9l6ZQeUAP2COUk2Z8buHkUkxiWHkES/ifbVuDeEisCFKnkOOr9N6tmGR4xv3CWT/GGLOezTp3ekAHsTcP13bOtUJip62ztInmJpuqycwTEnW8YA5/KRD5uYbR29pSt4UMwevDJuoIc2mhNtp6iNXlpBfWgKNstHAytN17IV8ITlPWMZc+NKca6Qwmh0XU067aITgPiUSbHC9YgwnAUsuciqMns8W7HKsdfRTHYAYDStiSpXTeZlbLHIFlXnGILIs4CVbzusP1IQf+MA4ITrzwPBW4XKyuIQAH+MTC62iTuDraVfbfLP6q8X4sooQrtAhhpU0BWGrxs8bzDOOF3EV6ReUVvuSXHWvDHWWYAOe08WJSxQqM0qfcmsMo1O2qxLo52AZFyxMXaCPkjqh4b8QFLuUHHORjCyp1LSHkK4vhGDo5KEGlHp82zLWtGas5SD0xASiLDY4Qb+hrVc0BwZDqqZZqobQjaq56wBAkl3GhDK+ExkoTJGpzUKQ2DbJZyMK9vu1pRCr1KOAQe9msxYLC6Y/Crb5ReKIrkhwTJF4W72i7CsUI82QRE1Kqt3fyA+LC3pLVXdXSJl3q6ujXwsLM4x08Q3FhzjCpu4+LyR1qMkJwXvPMAog1UXImAy2aXQcyNP+/exokfEjHXGeM1hVm9saYepIs8yNHzNWccs+4eLVPtDnNdRvpooUnrs0aADBDoPNTWpUT397CrV7EG3iKrACsF1ZByFapdUPy+sU2jncPAYAWB+5Dw3K03oV0Md88ly4tAVgp3BiJ5UhwDjFNM4C2eh2AEp4XnP7G4zFN1F/knV/QgqUeBABW8pGQyL7K/X+q4Mz1QhwgMgD6wtXZeGSFEz0rMg9O8wfwBwAfaCkv0y2RgWgohWcFJ5sjfgEAxgvbrjnDXLY7RXxydVTTMTwNwDYWCM5kze1tonR+qQHyvZ8AgMmcs5STmOii1WzRbh5zwF8AHNRU1uE0i7LNTGG8wERhavsAAHhJHFSQaJMtaEeJ2mzfwmLwHwDHeElwetIxihaHaTHg57x8rELI8Gw3sFtYApu5N0Jn6rFHJEFcLLdP4UaRlLFDy2V1Q80tYZ8TZtu5j+b37x/B2s5W/qLjoQijM1csaF/TwuZbglK6i61Uw090ItKhGE+Ljx8bah9XhdOffNZ9Ih+ktFOz4H/DDxxk+3rjC4DYLYKlVt0Okdz2CR1swiD12OmFvL9YTwE4w0jBuZ+hNnKv+LD7UY0AgFU8ZsWNlsK39wDrRbLErgo+UPvq6FR0ruQm6qzYPpZa6hPVVlFfoJIr9QvhqH/RCk7UdtCfhNWO12r7+qP70QNQXe0WXcxTNcvGxxntq3OqGgCoLtpbfuVyzWrBP63uqIoAoJaJjharuxXqCQ2YMf7pvjsAGqrvRVc/VEkqX/A2V1r2zzUHAPVb7W3LUycuqLZ+6r5bABLUFxGsqxn+6r6KUhG6Qg+yzjE7FPJo7aezBKMxhiSt5y1D7VC/dR/XIwDqsM0hYrxQ+8a0SgIAXfmnDbeQlsYvyqrIFAD4l/YJBcBoP3b/6kYAJLGdWy2cta4OWPD5CIDvGG4pn7X1EFRhAGC5JdlxUjnOG/PlFACoz9bL02ADHY2ZwlVyBEDh5WNxzjDAv90v3/H6OcwHxnkx8SV6Kt/p8iNRvImv6f8DACDkgI1cXcYWAAAAAElFTkSuQmCC",
    "menuText": localConstant.sideMenu.VISIT,
    "module": localConstant.moduleName.VISIT,
    "name": "mnuVisit",
    "subMenu": [
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuCreateVisit",
        "menuText": localConstant.visit.CREATE_VISIT,
        "viewUrl": "Visit/SearchAssignment",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.visit.CREATE_VISIT_MODE
      },
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuVisitSearch",
        "menuText": localConstant.visit.EDIT_VIEW_VISIT,
        "viewUrl": "EditVisit",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.visit.EDIT_VIEW_VISIT_MODE
      },
      //added for visit reports
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuUnapprovedVisitCHC",
        "menuText": "Print Unapproved Visits Report - CHC",
        "viewUrl": "UnapprovedVisitCHC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "UnapprovedVisitCHC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'UnapprovedVisitCHC',
      },
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuUnapprovedVisitOC",
        "menuText": "Print Unapproved Visits Report - OC",
        "viewUrl": "UnapprovedVisitOC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "UnapprovedVisitOC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'UnapprovedVisitOC',
      },
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuApprovedVisitProforma",
        "menuText": "Print Approved Visits Report - Proforma",
        "viewUrl": "ApprovedVisitProforma",
        "menuFun": 'HandleMenuAction',
        "currentPage": "ApprovedVisitProforma",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'ApprovedVisitProforma',
      },
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuApprovedVisitProformaNDT",
        "menuText": "Print Approved Visits Proforma - NDT ",
        "viewUrl": "ApprovedVisitProforma",
        "menuFun": 'HandleMenuAction',
        "currentPage": "ApprovedVisitProformaNDT",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'ApprovedVisitProformaNDT',
      },
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuVisitKPIReportCHC",
        "menuText": "Visits KPI Report - CHC",
        "viewUrl": "VisitKPIReportCHC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "VisitKPIReportCHC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'VisitKPIReportCHC',

      },
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuVisitKPIReportOC",
        "menuText": "Visits KPI Report - OC",
        "viewUrl": "VisitKPIReportOC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "VisitKPIReportOC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'VisitKPIReportOC',
      },
      {
        "module": localConstant.moduleName.VISIT,
        "name": "mnuVisitDownloadedReport",
        "menuText": localConstant.visit.VISIT_DOWNLOADED_REPORT,
        "viewUrl": "downloadServerReport",
        "menuFun": "HandleMenuAction",
        "isReportMenu":true,
        "currentPage": localConstant.visit.VISIT_DOWNLOADED_REPORT_MODE,
      },
    ]
  },
  {
    "menuIcon": "data:image/png;base64,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",
    "menuText": localConstant.sideMenu.TIMESHEET,
    "module": localConstant.moduleName.TIMESHEET,
    "name": "mnuTimesheet",
    "subMenu": [
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuCreateTimesheet",
        "menuText": localConstant.timesheet.CREATE_TIMESHEET,
        "viewUrl": "Timesheet/SearchAssignment",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.timesheet.CREATE_TIMESHEET_MODE
      },
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuTimesheetSearch",
        "menuText": localConstant.timesheet.EDIT_VIEW_TIMESHEET,
        "viewUrl": "EditTimesheet",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE
      },

       //added for timesheet reports
       {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuUnapprovedTimesheetCHC",
        "menuText": "Print Unapproved Timesheet Report - CHC",
        "viewUrl": "UnapprovedTimesheetCHC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "UnapprovedTimesheetCHC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'UnapprovedTimesheetCHC',
      },
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuUnapprovedTimesheetOC",
        "menuText": "Print Unapproved Timesheet Report - OC",
        "viewUrl": "UnapprovedTimesheetOC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "UnapprovedTimesheetOC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'UnapprovedTimesheetOC',
      },
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuApprovedTimesheetProforma",
        "menuText": "Print Approved Timesheet Report - Proforma",
        "viewUrl": "ApprovedTimesheetProforma",
        "menuFun": 'HandleMenuAction',
        "currentPage": "ApprovedTimesheetProforma",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'ApprovedTimesheetProforma',
      },
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuApprovedTimesheetProformaNDT",
        "menuText": "Print Approved Timesheet Proforma - NDT ",
        "viewUrl": "ApprovedTimesheetProforma",
        "menuFun": 'HandleMenuAction',
        "currentPage": "ApprovedTimesheetProformaNDT",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'ApprovedTimesheetProformaNDT',
      },
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuTimesheetKPIReportCHC",
        "menuText": "Timesheet KPI Report - CHC",
        "viewUrl": "TimesheetKPIReportCHC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "TimesheetKPIReportCHC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'TimesheetKPIReportCHC',

      },
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuTimesheetKPIReportOC",
        "menuText": "Timesheet KPI Report - OC",
        "viewUrl": "TimesheetKPIReportOC",
        "menuFun": 'HandleMenuAction',
        "currentPage": "TimesheetKPIReportOC",
        "isReportMenu":true,
        "isOpenModalPopup":true,
        "modalPopupCallback":'TimesheetKPIReportOC',
      },
      {
        "module": localConstant.moduleName.TIMESHEET,
        "name": "mnuTimesheetDownloadedReport",
        "menuText": localConstant.timesheet.TIMESHEET_DOWNLOADED_REPORT,
        "viewUrl": "downloadServerReport",
        "menuFun": "HandleMenuAction",
        "isReportMenu":true,
        "currentPage": localConstant.timesheet.TIMESHEET_DOWNLOADED_REPORT_MODE,
      },
    ]
  },
  {
    "menuIcon": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAQAAABpN6lAAAAACXBIWXMAAA3XAAAN1wFCKJt4AAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAuDSURBVHja3F17dE1XGv/lgaAZDULiLRXvwWqH0nqE0mbUczSixqMWplVlDGbNlEaZTk2r7RpmaTFjPLpYlGGsFksV1WXGM8y0Q0enSjTEKyUJQhLJb/5wc3Me+5yzz7nnJOf69h/3nLP3/s7+fmc/v+/b+4JwHJL5F96mnv7LaYwMgW+lhlAy76ExDQoXACLhlFriKZPYyQgTcg5AwxBiHwgAHhCSAaAb3sMARIT4pjjMxCzU9R0CFp1ENDcGurV/so4qpjvN6LCGT2pgvCjkkHAaBaK5WSVUHYcADOTdYEyRvyCQF18JQRSHcr8pALf4Dh8J8HlWIb7vILAj/n0IWnMOv6cMlXEnB3EYi3QxPoLAnvjukW8gEI8C0diA5zzte6tjM4b4dRj0XnwfQRBB/bMXsLqS3n4DjXHHfzXgpUp7exzSq7oGRAueZeNxWzzycAk5KEQLPIJaNt+f7ceZYG+WSfXkxdzNV9hMlTeBPbmEBZJjwUlG+HMYnGIJQSnXakRXhjqcyXOW4p9lc//OA8whOMJOlqyj+Jap+N+ZAOiLmaAxBOsYI0hfXfBsBG8a8DjDpv5fCywQFj1DlSaRs7mKh3iD5BUe5Ur2VrXrdjwj4HGbTcJhMbReUPRliviHuVCoFD3DsSoIRGm6+h+ABoJFzOeMDsaP5HWTFr6F9YMpXxDEr/E/AK8KlrgJwdhRvGfRx59hw2DqD3Wxd1jPHwCIpsIAEIWzaKZ5tgDzA1dp2IAoyynGCaTgJgCgNnqhAAW4iduIR2M0QRN8imP+Uokl8U+8zLxAyNd9syt8KJCyhaBxiBa869jKAv/aHMc9/IYnOFM4slRqEzhoIdBbwZSrLIW/wAxFAzAPEezHjSzijKoFoKOlUE8EUraxaP2ZTFN0lbIhns9WLQCpFuJfCVr7lpsqwRY5EL5KQ7RkV3EQZYGrHoZprmM8toebYUQWgJzAbwzaG6Q4hHSDxW0EuqAVEpGIBNzBJVxCDo7jSngC0Mkgx0pMwT3d01p4GoMwEImCwScT27EDx/0yDFr1AS8G0vUXxu5ilK511eRs5lp2rQfZJzzM4wmB38uCuNNIR6mmXr2IM3gH9Sz59sB+7ERn/9eANcHhSkvXmaxBtS732rQSlHCq32tAUuA3F8Wq5/eQhm9VT9rjKPrZ7omWYjmq+bkGXDBY2PxWg+jTgkm0LO3X2J8rdSLU1bJ4XQIpOyueXWJNFbt20upQMe2ofOeqisuTFoX7IJiyooWr5+8/4umQbYZvVB0AT3C3qSI0n7UDKR/nHcH3j+A2F4ymZRxelQqROCYEQhPBGD45mG4I7+m+/xSX7MYFbOAPjdAiXdFy2TgYO5E5qu8fw4uumc7f9QcASSzVFe0LxYwvSZV6uou+A7crsw6YRe4QFG6BMGUMc1x1n6jEOmCkEwSAWXhX96wMM7FE93Qqlro6OSlEM/ygetIWbQXpruCQd25yqRrXpgpaqbMC7XHdhWa0iv9WE/NqijdNwFh8kjzAWNW6767rAKyRBIA85MVaIBXbUMOk2vRUKcV7m6Z1RgNUd2Zag+5o47aHSCcL8YGLyDMsrDvUCB0Vd5kWfiYuAzDL8oueNPlaXtSBE5XrI9TNMpcagGRPSqbkes22BjEF0yXUMQYArLTMdVZx/RBqegJAA9Vdro2cLfAlPscSXMAcZwCswBGLXPmK63iP6ma8RuUuL/5+dAposN/EAicA3MIzFhDkGX4pr2qAGQA/RaxK/OaKuHn4nbOJUB0eNhl7n1SkHOSRN3GuqjyrLFaQ77MDwRbMsqtjAO1BkMXB/JBtFOkmeARAmUo79J6USi3LIOb3zhZDegiy2EKXarRn/uTKt7wfIrc3nWiF8zV9wXmkIEtgEfSG1IuheiFym4OF8p2gGAKx+N4BkOsqAMCrmGcfACAffTAeOzEdnYXia7+UX2vAfQjq2lkOy4Y4j/qAzaq3fO8KzynO/QPM6kiZJ9svRU2gFJdxEUVohEaO5p9HnJvHjakM/xNqa0Kl04rr6jiKXdiFkwojbBxSMByDbKwFDwoXVS7o1VZ70gR6SL07mun8Vorf1wovR5e2z5eHlzwQv9iG21w0p/CqBb9TRl5rbgDQxQMAjtssQ1P+y3RjRoNQnaTMtQOFtrfKWNFRwepwFNqiNZIRi0vIwWF8gkyUK7Wz0RPrMMygfP1wzb5W2E74wvUaMEHFvzWXB+yR2j0nYxTO+ZH8WMhru1dHaFSE37gsfikTFdz7CmxUysZS4aESK7Rxl7Kl1wA0k9xmJUu7Vdz/bJH6OvsrDHYiJ/5FXgPgdiMYp9p7dE1izKgwj/xKqFuIcWIas0O/wAoXDWMNcSt41x+fSc0bu+FcYNJ0Gi118W/goGKVccztThCsK+VCL0frVZxXSOY6EMzxvGXa/3CA203A3Hxlj55RNYCr0vmGBfevFUhA4DoAj7nUER5RcX3KRs6TwVybJFJ3DfUgJS0dxxZX+Kh1+aNt5OyADoGrTyRS15NTiNih1zQOs05oL/aqdPxjbeUeHvg9EJppzCl9g7Uuf/8Mm96j3QO/OWBVAADMx92Q8m9VrQFaYZxtm/J9KpY3pbkLQDZmhZD7Kl5R3b9ue6mWELy6VDUAAB/gbw5zEmNVxW5nqwPU2ixjqwoAYFJgRmaX/oDdmuZkv2zlfutRaFp1AOQjXeNSL0P/0Gntt2GVgSLemM4EfpvKNx4v9LnHMEGwf8iMTmGkbgjdgIloiSRMwlaUSPO5Ty1tNT1PQipvSc/gDjDOglsCX5OwDOQHfdemSbw11e2psDZ0lZzF/11S/RnFoRZbcRYH056Q2KTTyGsAwGR+Zan5WSzYb2YWBhruSSgNnl7XTQL2re4vhkQhkuNMTpPZxg4OeFbjDN4QcJtr6kxxJ7gvPo/fcS7j3QTgYT7JyVzMz4SOCNU5jed1FXCf0PCRyF3czPlMYzvTPcj1uVbDcVMwrqHwyI6e3qjEfswMHle9aLVBwesxhdO4nG9zDDuzhjBNG1VtucUtHMu6hu+epNATZwZ3s0Rwl0D8r9zXCTbjIuH5MOR21nLEsbtwn2kJ93Gi8Ige8FGeDexmquhGZ8tahUMBIJl/ZbFJB3NYt5XSOowXVtxyyuY0zf60ctP8eg5VjTyict1UuXaHCEAyN5pq6cu7nAyDrybmKbPT9Ap/zWqmfDozW0LHGAIAEZzBQunpzdfsLcGzBufZcLb/NzsbcvqZ4dTL9PhOefGTHOj+M/lLk/0/3bhUYn+51gKQIexmM0w1kiYQyNoFxmA5ajtaGdzDbnyKLJzHeeQhEo3QHM3RBumO/fwzMRwXVE8isBOppnmKkYaPna8F3LL9FZh2nvKUrZtCxVtu2yviYGdNIJJL6D+6rpvapFh2zpdFHbO198VH9CfdUQ2AIPi6ZZ7n7QOwjP6lu+yuqav7LHIMtgvAVPqbLmtOpUw0XYKfF23ONxO/P0vod/oyeMLZ/TDYMOVN8YLIWPyWpucF+oe2asp9xI74ZgDsZLhQmqrcw+yIbwzAcwwfuqBqBhE8JS++EQCxvMBwIvVu87Hy4hsB8EeGF5Wwo2ruck5WfDEADVx0d6ks+kglwcuy4osBmMvwoxLVScUxvMxS7pA5vFe/GozGOTRB+NFCzFXcxaEWLspk0wMwwrF9t2opF02deCfobYMvIzypPkY5yaYFoDH6Ilzp524AkBbyf4pVHfV1spFbC8BIhC9FYUSoADQL+lmFJ40MFYCRYdwAAKCPwk3KEQDpCG+KtN8IlAAk4ScId0oPBYDHEP70aCgAJD4AANS2u3/tQQPA9v/dKgG49gCIX6Q64cYmAGtDdHX2A22ye6CDEoAfsCnsAVgW2jxgruaQrHCjt+0fsKjVB9TBFtP/FPcvlWKqk617eoVINaShF3qhfdhMiwtxCAew3dl/Ffx/AJPGEB8s3kY1AAAAAElFTkSuQmCC",
    "menuText": localConstant.sideMenu.ADMIN,
    "module": localConstant.moduleName.SECURITY,
    "name": "mnuSecurity",
    "subMenu": [
      {
        "module": localConstant.moduleName.SECURITY,
        "name": "mnuRole",
        "menuText": "User Roles",
        "viewUrl": "UserRoles",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.admin.VIEWROLE
      },
      {
        "module": localConstant.moduleName.SECURITY,
        "name": "mnuUser",
        "menuText": "Users",
        "viewUrl": "Users",
        "menuFun": 'HandleMenuAction',
        "currentPage": localConstant.security.VIEWUSER
      },
      {
        "module": localConstant.moduleName.AUDIT,
        "name": "mnuAuditSearch",
        "menuText": "Audit Search",
        "viewUrl": "AuditSearch",
        "menuFun": 'HandleMenuAction',
        "currentPage": "AuditSearch"
      },
      // {
      //   "module": localConstant.moduleName.SECURITY,
      //   "name": "mnuRole",
      //   "menuText": "Manage Reference Types",
      //   "viewUrl":  "ManageReferencesType",
      //   "menuFun":  'HandleMenuAction',
      //   "currentPage": "Manage Reference Types"
      // },
      // {
      //   "module": localConstant.moduleName.SECURITY,
      //   "name": "mnuRole",
      //   "menuText": "Assignment Lifecycle and Status",
      //   "viewUrl":  "AssignmentLifecycleAndStatus",
      //   "menuFun":  'HandleMenuAction',
      //   "currentPage": "Assignment Lifecycle and Status"
      // }
    ]
  }
];