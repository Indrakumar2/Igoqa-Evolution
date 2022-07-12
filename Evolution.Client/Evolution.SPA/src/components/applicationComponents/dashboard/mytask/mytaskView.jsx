import React,{ Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './header';
import TechSpecMyTask  from "../../../../grm/components/techSpec/techSpecMytask";

class MyTaskView extends Component{
    render(){   
        
        return(
            <TechSpecMyTask/>
        );
    }
}

export default MyTaskView;