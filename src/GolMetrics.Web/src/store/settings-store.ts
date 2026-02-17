import { create } from 'zustand';
import { toast } from 'sonner';
import type { UserProfile } from '@/types';
import * as settingsService from '@/services/settings-service';

interface SettingsState {
  profile: UserProfile | null;
  isLoadingProfile: boolean;
  isSubmittingApiKey: boolean;
  fetchProfile: () => Promise<void>;
  submitApiKey: (key: string) => Promise<void>;
}

export const useSettingsStore = create<SettingsState>()((set, get) => ({
  profile: null,
  isLoadingProfile: false,
  isSubmittingApiKey: false,

  fetchProfile: async () => {
    set({ isLoadingProfile: true });
    try {
      const profile = await settingsService.getProfile();
      set({ profile });
    } catch {
      toast.error('Failed to load profile');
    } finally {
      set({ isLoadingProfile: false });
    }
  },

  submitApiKey: async (key: string) => {
    set({ isSubmittingApiKey: true });
    try {
      await settingsService.updateApiKey(key);
      toast.success('API key saved successfully');
      await get().fetchProfile();
    } catch (error: unknown) {
      const axiosError = error as { response?: { status?: number; data?: { detail?: string } } };
      const status = axiosError.response?.status;
      const detail = axiosError.response?.data?.detail;

      if (status === 502) {
        toast.error('API key validation service is unavailable. Please try again later.');
      } else if (status === 400) {
        toast.error(detail ?? 'The provided API key is not valid.');
      } else {
        toast.error('Failed to save API key');
      }
    } finally {
      set({ isSubmittingApiKey: false });
    }
  },
}));
