import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import {
    addProcedureToPlan,
    getPlanProcedures,
    getProcedures,
    getUsers,
    getUsersByProcedure, // updated API call
} from "../../api/api";
import Layout from "../Layout/Layout";
import ProcedureItem from "./ProcedureItem/ProcedureItem";
import PlanProcedureItem from "./PlanProcedureItem/PlanProcedureItem";

const Plan = () => {
    const { id } = useParams();
    const [procedures, setProcedures] = useState([]);
    const [planProcedures, setPlanProcedures] = useState([]);
    const [users, setUsers] = useState([]);

    useEffect(() => {
        (async () => {
            const proceduresData = await getProcedures();
            const planProceduresData = await getPlanProcedures(id);
            const usersData = await getUsers();

            // Convert users to select options
            const userOptions = usersData.map((u) => ({
                label: u.name,
                value: u.userId,
            }));
            setUsers(userOptions);
            setProcedures(proceduresData);

            // Load assigned users for each plan procedure
            const planProceduresWithUsers = await Promise.all(
                planProceduresData.map(async (pp) => {
                    const assignedUsersData = await getUsersByProcedure(pp.id);
                    const selectedUsers = assignedUsersData.map((u) => ({
                        label: u.user.name,
                        value: u.userId,
                    }));
                    return { ...pp, assignedUsers: selectedUsers };
                })
            );

            setPlanProcedures(planProceduresWithUsers);
        })();
    }, [id]);

    const handleAddProcedureToPlan = async (procedure) => {
        const exists = planProcedures.some((p) => p.procedureId === procedure.procedureId);
        if (exists) return;

        await addProcedureToPlan(id, procedure.procedureId);
        setPlanProcedures((prev) => [
            ...prev,
            {
                planId: id,
                procedureId: procedure.procedureId,
                id: null, // backend will assign Id
                procedure: {
                    id: procedure.id,
                    procedureId: procedure.procedureId,
                    procedureTitle: procedure.procedureTitle,
                },
                assignedUsers: [],
            },
        ]);
    };

    return (
        <Layout>
            <div className="container pt-4">
                <div className="d-flex justify-content-center">
                    <h2>OEC Interview Frontend</h2>
                </div>
                <div className="row mt-4">
                    <div className="col">
                        <div className="card shadow">
                            <h5 className="card-header">Repair Plan</h5>
                            <div className="card-body">
                                <div className="row">
                                    <div className="col">
                                        <h4>Procedures</h4>
                                        <div>
                                            {procedures.map((p) => (
                                                <ProcedureItem
                                                    key={p.procedureId}
                                                    procedure={p}
                                                    handleAddProcedureToPlan={handleAddProcedureToPlan}
                                                    planProcedures={planProcedures}
                                                />
                                            ))}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <h4>Added to Plan</h4>
                                        <div>
                                            {planProcedures.map((p) => (
                                                <PlanProcedureItem
                                                    key={p.procedure.procedureId}
                                                    procedure={p.procedure}
                                                    users={users}
                                                    assignedUsers={p.assignedUsers} // pass selected users
                                                    planProcedureId={p.id} // needed for assign/remove calls
                                                />
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </Layout>
    );
};

export default Plan;
