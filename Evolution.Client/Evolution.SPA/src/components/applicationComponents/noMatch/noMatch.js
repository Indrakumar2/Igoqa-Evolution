import React,{ Component } from 'react';
import UnderConstruction from '../../../assets/images/page-under-construction.png';
class NoMatch extends Component{
    render(){ 
        return(
            <div className="customCard p-4 construction">
               <img src={UnderConstruction}  alt="Intertek Logo" />
            </div>
        );
    }
}

export default NoMatch;