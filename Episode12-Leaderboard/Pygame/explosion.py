import pygame
import shared

class Explosion():
	def __init__(self, images:list, centre:tuple, key:str) -> None:
		''' Class variables. key: 'sm', 'lg', 'player '''
		self.images = images				# list of 8 images
		self.centre = centre				# use for all frames					# 
		self.image = images[key][0] 		# set to first image in the sequence
		self.image = images[self.key][0] 	# set to first image in the sequence
		self.rect = self.image.get_rect()	# define rectangle from image size
		self.rect.center = self.centre		# set centre for all frames
		self.frame = 0						# no of first frame
		self.time_passed = 0				# set timer to 0
		self.frame_rate = 0.1 				# 8 images played at 1 frame per 0.1 secs = 0.8 seconds
		self.active = True

	def update(self, dt):
		self.time_passed += dt
		if self.time_passed >= self.frame_rate:					# 0.1 seconds has passed
			self.time_passed = 0								# reset timer
			self.frame += 1										# increase frame number
			if self.frame >= len(self.images[self.key]):		# check if end of list?
				self.active = False								# animation finished
			else:
				self.image = self.images[self.key][self.frame] 	# next frame
				self.rect = self.image.get_rect()				# new rectangle
				self.rect.center = self.centre					# set centre to parameter value

		return self.active

	def draw(self):
		shared.screen.blit(self.image, self.rect)				# draw current frame