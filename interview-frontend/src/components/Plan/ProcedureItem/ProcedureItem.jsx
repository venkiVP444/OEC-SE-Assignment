import React from "react";

const ProcedureItem = ({ procedure, handleAddProcedureToPlan, planProcedures }) => {
    const isChecked = planProcedures.some(p => p.procedureId === procedure.procedureId);
    const checkboxId = `procedureCheckbox-${procedure.procedureId}`; // unique id

    return (
        <div className="py-2">
            <div className="form-check">
                <input
                    className="form-check-input"
                    type="checkbox"
                    id={checkboxId}
                    checked={isChecked}
                    onChange={() => handleAddProcedureToPlan(procedure)}
                />
                <label className="form-check-label" htmlFor={checkboxId}>
                    {procedure.procedureTitle}
                </label>
            </div>
        </div>
    );
};

export default ProcedureItem;
