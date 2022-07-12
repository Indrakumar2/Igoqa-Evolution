import React,{ Component } from 'react';

class Extranet extends Component{
    render(){
        return(
            <div className="genralDetailContainer">
                <h6>Extranet</h6>
                <div className="formCard">
                    <div className="row">
                        <div className="input-field col s12">
                            <textarea id="technical_specialist_home_page" className="materialize-textarea" />
                            <label htmlFor="technical_specialist_home_page">Resource Home Page Comment</label>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default Extranet;
