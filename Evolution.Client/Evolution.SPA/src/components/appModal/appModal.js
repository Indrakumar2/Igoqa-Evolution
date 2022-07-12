import React, { Component } from 'react';
import MaterializeComponent from 'materialize-css';

class AppModal extends Component {
    componentWillMount(){
            const modal = document.querySelectorAll('.modal');
            const modalInstances = MaterializeComponent.Modal.init(modal,{ "dismissible":false });
    }
    render() {
        return (
            <div id="detailModal" className="modal">
                <div className="modal-content">
                <h4>Intertek</h4>
                <p>Details Here</p>
                </div>
                <div className="modal-footer">
                    <a href="#!" className="modal-close waves-effect waves-green btn-flat">Ok</a>
                </div>
            </div>
        );
    }
}

export default AppModal;