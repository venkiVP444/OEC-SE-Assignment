import React, { useState, useEffect } from "react";
import ReactSelect from "react-select";
import {
    assignUserToProcedure,
    getUsersByProcedure,
    removeUserFromProcedure,
    removeAllUsersFromProcedure
} from "../../../api/api";

const PlanProcedureItem = ({ procedure, users, planProcedureId }) => {
    const [selectedUsers, setSelectedUsers] = useState([]);
    const [loading, setLoading] = useState(false);

    // Load assigned users on refresh/page load
    useEffect(() => {
        if (!planProcedureId) return;

        const loadAssignedUsers = async () => {
            setLoading(true);
            try {
                const assigned = await getUsersByProcedure(planProcedureId);
                const formatted = assigned.map(a => ({
                    value: a.userId,
                    label: a.user.name,
                    id: a.id // backend row id
                }));
                setSelectedUsers(formatted);
            } catch (err) {
                console.error("Error loading assigned users:", err);
            } finally {
                setLoading(false);
            }
        };

        loadAssignedUsers();
    }, [planProcedureId]);

    // Handle user selection changes
    const handleChange = async (selectedOptions) => {
        if (!planProcedureId) return;

        const added = selectedOptions.filter(
            s => !selectedUsers.some(u => u.value === s.value)
        );
        const removed = selectedUsers.filter(
            u => !selectedOptions.some(s => s.value === u.value)
        );

        setLoading(true);
        try {
            // Assign added users
            for (const user of added) {
                const result = await assignUserToProcedure({
                    planProcedureId,
                    userId: user.value
                });
                user.id = result.id; // store backend DB row id
            }

            // Remove deleted users
            for (const user of removed) {
                await removeUserFromProcedure(user.id);
            }

            setSelectedUsers(selectedOptions.map(u => {
                const existing = selectedUsers.find(s => s.value === u.value);
                return existing ? existing : u;
            }));
        } catch (err) {
            console.error("Error updating users:", err);
        } finally {
            setLoading(false);
        }
    };

    // Remove all users at once
    const handleRemoveAll = async () => {
        if (!planProcedureId) return;

        setLoading(true);
        try {
            await removeAllUsersFromProcedure(planProcedureId);
            setSelectedUsers([]);
        } catch (err) {
            console.error("Error removing all users:", err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="py-2 border-bottom mb-2">
            <div className="d-flex flex-column flex-sm-row justify-content-between align-items-start align-items-sm-center">
                <div className="me-sm-3 text-break">
                    <strong>{procedure.procedureTitle}</strong>
                </div>
                <button
                    className="btn btn-sm btn-danger mt-2 mt-sm-0 flex-shrink-0"
                    onClick={handleRemoveAll}
                    disabled={loading || selectedUsers.length === 0}
                >
                    Remove All
                </button>
            </div>


            <ReactSelect
                className="mt-2"
                placeholder="Select Users"
                isMulti
                options={users}
                value={selectedUsers}
                onChange={handleChange}
                isDisabled={loading}
            />
        </div>
    );
};

export default PlanProcedureItem;
