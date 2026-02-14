import type {StatsData} from "@/types";
import {Trophy} from "lucide-react";

interface StatCardProps {
    data: StatsData;
}

export const StatCard = ({data}: StatCardProps) => {
    return (
        <div className="relative overflow-hidden bg-black rounded-lg border border-gray-800 shadow-2xl w-full max-w-xl">
            <div
                className="h-1.5 w-full bg-gradient-to-r from-accent via-orange-400 to-orange-600 shadow-[0_0_20px_rgba(255,165,0,0.6)]"></div>

            <div className="p-6 md:p-8 relative">
                <div className="absolute inset-0 grid-bg pointer-events-none opacity-20"></div>

                <div className="relative z-10">
                    <div className="flex flex-col md:flex-row gap-8 items-start md:items-center">
                        <div className="flex flex-col">
                            <h3 className="text-accent text-sm font-bold tracking-[0.25em] mb-1 drop-shadow-[0_0_8px_rgba(255,221,87,0.6)] flex items-center gap-2 uppercase">
                                <Trophy className="w-4 h-4"/>
                                {data.title}
                            </h3>
                            <div
                                className="text-[5rem] leading-[0.9] font-bold text-white tracking-tighter drop-shadow-[0_0_20px_rgba(255,255,255,0.15)] font-display tabular-nums">
                                {data.value}
                            </div>
                        </div>

                        <div
                            className="hidden md:block w-px h-24 bg-gradient-to-b from-transparent via-gray-700 to-transparent"></div>

                        <div className="flex flex-col gap-4 flex-1 w-full md:w-auto">
                            <div className="space-y-3">
                                {data.leagues.map((league, idx) => (
                                    <div key={league.name}>
                                        <div className="flex items-center justify-between min-w-[180px] gap-4">
                      <span className="text-muted-foreground text-sm font-mono uppercase tracking-wider">
                        {league.name}
                      </span>
                                            <span className="text-white text-xl font-bold font-mono">
                        {league.value.toString().padStart(2, "0")}
                      </span>
                                        </div>
                                        {idx < data.leagues.length - 1 && (
                                            <div className="w-full h-px bg-gray-800 mt-3"></div>
                                        )}
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>

                    <div className="mt-6 pt-4 border-t border-gray-800">
                        <p className="text-gray-300 text-sm leading-relaxed font-light">
                            <span className="text-primary font-bold">Insight:</span>{" "}
                            {data.insight}{" "}
                            <span className="text-white font-bold">{data.efficiency}%</span>{" "}
                            en el area penal.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};
