export interface Message {
    id: string;
    role: "user" | "assistant";
    content?: string;
    type: "text" | "stats-card";
    timestamp: string;
    data?: StatsData;
}

export interface StatsData {
    title: string;
    value: number;
    leagues: {
        name: string;
        value: number;
    }[];
    insight: string;
    efficiency: number;
}
