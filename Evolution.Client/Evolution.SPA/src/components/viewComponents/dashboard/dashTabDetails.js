export const dashboardMenu=[
  {
    tabHeader:"My Searches",
    isActive:true,
    tabBodyComponent:"/Dashboard/MySearch",
    tabCount:"MySearchCount",
    isDisableMenu:[ 'TechnicalSpecialist' ]
  },
    {
      tabHeader:"My Tasks",
      isActive:true,
      tabBodyComponent:"/Dashboard/MyTasks",
      tabCount:"MyTaskCount",
      isDisableMenu:[ 'TechnicalSpecialist' ]
    },
    {
      tabHeader:"Active Assignments",
      isActive:false,
      tabBodyComponent:"/Dashboard/ActiveAssignments",
      tabCount:"AssignmentCount",
      isDisableMenu:[ 'TechnicalSpecialist','ResourceCoordinator','ResourceManager','TechnicalManager','OperationManager' ]
    },
    {
      tabHeader:"Inactive Assignments",
      isActive:false,
      tabBodyComponent:"/Dashboard/InactiveAssignments",
      tabCount:"InactiveAssignmentCount",
      isDisableMenu:[ 'TechnicalSpecialist','ResourceCoordinator','ResourceManager','TechnicalManager','OperationManager' ]
    },
    {
      tabHeader:"Visit Status and Approval",
      isActive:false,
      tabBodyComponent:"/Dashboard/VisitStatusAndApproval",
      tabCount:"VisitCount",
      isDisableMenu:[ 'TechnicalSpecialist','ResourceCoordinator','ResourceManager','TechnicalManager','OperationManager' ]
    },
    {
      tabHeader:"Timesheets Pending Approval",
      isActive:false,
      tabBodyComponent:"/Dashboard/TimesheetPendingApproval",
      tabCount:"TimesheetCount",
      isDisableMenu:[ 'TechnicalSpecialist','ResourceCoordinator','ResourceManager','TechnicalManager','OperationManager' ]
    },
    {
      tabHeader:"Contracts Near Expiry",
      isActive:false,
      tabBodyComponent:"/Dashboard/ContractsNearExpiry",
      tabCount:"ContractCount",
      isDisableMenu:[ 'TechnicalSpecialist','ResourceCoordinator','ResourceManager','TechnicalManager','OperationManager' ]
    },
    {
      tabHeader:"Document Approval",
      isActive:false,
      tabBodyComponent:"/Dashboard/DocumentApproval",
      tabCount:"DocumentApprovalCount",
      isDisableMenu:[ 'TechnicalSpecialist','ResourceCoordinator','ResourceManager','TechnicalManager','OperationManager' ]
    },
    // {
    //   tabHeader:"Document Approval",
    //   isActive:false,
    //   tabBodyComponent:"/Dashboard/DocumentApproval",
    //   tabCount:"DocumentAprovalCount"
    // },
    {
      tabHeader:"Budget Monetary",
      isActive:false,
      tabBodyComponent:"/Dashboard/BudgetMonetary",
      isDisableMenu:[]
    },
    {
      tabHeader:"Budget Hours",
      isActive:false,
      tabBodyComponent:"/Dashboard/BudgetHours",
      isDisableMenu:[]
    },
    {
      tabHeader:"Documentation",
      isActive:false,
      tabBodyComponent:"/dashboard/Documentation",
      isDisableMenu:[]
    }
  ];