const api_url = "http://localhost:10010";

// Start a new plan
export const startPlan = async () => {
    const url = `${api_url}/Plan`;
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({}),
    });

    if (!response.ok) throw new Error("Failed to create plan");

    return await response.json();
};

// Add a procedure to a plan
export const addProcedureToPlan = async (planId, procedureId) => {
    const url = `${api_url}/Plan/AddProcedureToPlan`;
    const command = { planId: planId, procedureId: procedureId };
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(command),
    });

    if (!response.ok) throw new Error("Failed to add procedure to plan");

    return true;
};

// Get all procedures
export const getProcedures = async () => {
    const url = `${api_url}/Procedures`;
    const response = await fetch(url, { method: "GET" });

    if (!response.ok) throw new Error("Failed to get procedures");

    return await response.json();
};

// Get procedures for a specific plan
export const getPlanProcedures = async (planId) => {
    const url = `${api_url}/PlanProcedure?$filter=planId eq ${planId}&$expand=procedure`;
    const response = await fetch(url, { method: "GET" });

    if (!response.ok) throw new Error("Failed to get plan procedures");

    return await response.json();
};

// Get all users
export const getUsers = async () => {
    const url = `${api_url}/Users`;
    const response = await fetch(url, { method: "GET" });

    if (!response.ok) throw new Error("Failed to get users");

    return await response.json();
};

// Get users assigned to a specific procedure
export const getUsersByProcedure = async (planProcedureId) => {
    const url = `${api_url}/PlanProcedure/byProcedure/${planProcedureId}`;
    const response = await fetch(url, { method: "GET" });

    if (!response.ok) throw new Error("Failed to get users for procedure");

    return await response.json();
};

// Assign a user to a procedure
export const assignUserToProcedure = async (model) => {
    const url = `${api_url}/PlanProcedure/assign`;
    const response = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(model),
    });

    if (!response.ok) throw new Error("Failed to assign user to procedure");

    return await response.json();
};

// Remove a user from a procedure
export const removeUserFromProcedure = async (id) => {
    const url = `${api_url}/PlanProcedure/remove/${id}`;
    const response = await fetch(url, { method: "DELETE" });

    if (!response.ok) throw new Error("Failed to remove user from procedure");

    return true;
};

// Remove all users from a procedure
export const removeAllUsersFromProcedure = async (planProcedureId) => {
    const url = `${api_url}/PlanProcedure/removeAll/${planProcedureId}`;
    const response = await fetch(url, { method: "DELETE" });

    if (!response.ok) throw new Error("Failed to remove all users from procedure");

    return true;
};
